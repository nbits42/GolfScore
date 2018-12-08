#define OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TeeScore.Services
{
    public class DataService : IDataService
    {
        private MobileServiceSQLiteStore _store;
        private const string dbVersion = "0.2";

        public DataService()
        {
        }

        public async Task InitializeAsync()
        {
            var file = DependencyService.Get<IDatabaseConnection>().DbConnection(dbVersion);

            _store = new MobileServiceSQLiteStore(file);
            _store.DefineTable<Game>();
            _store.DefineTable<Player>();
            _store.DefineTable<GamePlayer>();
            _store.DefineTable<Venue>();
            _store.DefineTable<VenueAvailabilityPeriod>();
            _store.DefineTable<VenueFacilities>();
            _store.DefineTable<Facility>();
            _store.DefineTable<Availability>();
            _store.DefineTable<AvailabilityPeriod>();

#if OFFLINE_SYNC_ENABLED

            await App.MobileService.SyncContext.InitializeAsync(_store).ConfigureAwait(false);

            var players = await PlayersTable.ToListAsync();
            if (players.Count == 0)
            {
                await SyncAsync().ConfigureAwait(false);
            }
#endif

            MessagingCenter.Send(this, ServiceMessage.DataServiceInitialized);
        }

        public bool IsOnline
        {
            get
            {
                var current = Connectivity.NetworkAccess;
                return current == NetworkAccess.Internet;
            }
        }

        private static string NewId()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<List<Game>> GetGames(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
            {
                return new List<Game>();
            }
            var gamePlayers = await GamePlayersTable.Where(x => x.PlayerId == playerId && x.Hide == false).ToListAsync().ConfigureAwait(false);
            var result = new List<Game>();
            foreach (var gamePlayer in gamePlayers)
            {
                var game = await GamesTable.LookupAsync(gamePlayer.GameId).ConfigureAwait(false);
                if (game != null)
                {
                    result.Add(game);
                }
            }

            return result;
        }

        public async Task<Venue> GetVenue(string venueId)
        {
            if (string.IsNullOrEmpty(venueId))
            {
                return null;
            }
            return await VenuesTable.LookupAsync(venueId).ConfigureAwait(false);
        }

        public async Task<List<Player>> GetPlayersForGame(string gameId)
        {
            var gamePlayers = await GamePlayersTable.Where(x => x.GameId == gameId).ToListAsync().ConfigureAwait(false);
            var result = new List<Player>();
            foreach (var gamePlayer in gamePlayers)
            {
                result.Add(await PlayersTable.LookupAsync(gamePlayer.PlayerId).ConfigureAwait(false));
            }

            return result;
        }

        public async Task SyncAsync()
        {
            if (!IsOnline)
            {
                return;
            }
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await App.MobileService.SyncContext.PushAsync().ConfigureAwait(false);
                await GamesTable.PullAsync("AllGames", this.GamesTable.CreateQuery()).ConfigureAwait(false);
                await PlayersTable.PullAsync("AllPlayers", this.PlayersTable.CreateQuery()).ConfigureAwait(false);
                await VenuesTable.PullAsync("AllVenues", this.PlayersTable.CreateQuery()).ConfigureAwait(false);
                await GamePlayersTable.PullAsync("AllGamePlayers", this.GamePlayersTable.CreateQuery()).ConfigureAwait(false);
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result).ConfigureAwait(false);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync().ConfigureAwait(false);
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.",
                        error.TableName, error.Item["id"]);
                }
            }
        }

        public async Task<Player> GetPlayer(string myPlayerId)
        {
            return await PlayersTable.LookupAsync(myPlayerId).ConfigureAwait(false);
        }

        public async Task<Player> SavePlayer(Player player, bool synchronize = false)
        {
            return await SaveAsync<Player>(player, synchronize).ConfigureAwait(false);
        }

        public async Task<Game> SaveGame(Game game, bool synchronize = false)
        {
            return await SaveAsync<Game>(game, synchronize).ConfigureAwait(false);
        }

        public async Task<Venue> SaveVenue(Venue venue, bool synchronize = false)
        {
            return await SaveAsync<Venue>(venue, synchronize).ConfigureAwait(false);
        }

        private async Task<T> SaveAsync<T>(DomainBase entity, bool synchronize = false) where T : DomainBase
        {
#if OFFLINE_SYNC_ENABLED
            var table = App.MobileService.GetSyncTable<T>();
#else
            var table = App.MobileService.GetTable<T>();
#endif
            var action = "init";
            try
            {

                if (entity.IsNew)
                {
                    entity.Id = NewId();
                    action = "insert";
                    await table.InsertAsync((T)entity).ConfigureAwait(false);
                }
                else
                {
                    action = "update";
                    await table.UpdateAsync((T)entity).ConfigureAwait(false);
                }
                action = "sync";
                if (synchronize)
                {
                    await SyncAsync();
                }
                action = "lookup";

                return await table.LookupAsync(entity.Id).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var msg = $"Error executing {action} operation. Item: {typeof(T).Name} ({entity.Id}). Operation discarded. ";
                ErrorReportingService.ReportError(this,e, msg);
                //Debug.WriteLine($"{msg}. Error: {e.Message}");
                throw;
            }
        }

        public async Task<List<Venue>> GetVenues()
        {
            return await VenuesTable.ToListAsync().ConfigureAwait(false);
        }

        public async Task<GamePlayer> SaveGamePlayer(GamePlayer gamePlayer, bool synchronize = false)
        {
            return await SaveAsync<GamePlayer>(gamePlayer, synchronize);
        }

        public async Task<GamePlayer> GetGamePlayer(string gameId, string playerId)
        {
            var result = await GamePlayersTable.Where(x => x.GameId == gameId && x.PlayerId == playerId).ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<List<Player>> GetKnownPlayers(string playerId)
        {
            var myGames = await GetGames(playerId);
            var knownPlayers = new List<Player>();
            foreach (var game in myGames)
            {
                var players = (await GetPlayersForGame(game.Id)).Where(x => x.Id != playerId);
                foreach (var player in players)
                {
                    if (!knownPlayers.Exists(x => x.Id == player.Id))
                    {
                        knownPlayers.Add(player);
                    }
                }
            }

            return knownPlayers;
        }


#if OFFLINE_SYNC_ENABLED
        private IMobileServiceSyncTable<Game> GamesTable => App.MobileService.GetSyncTable<Game>();
        private IMobileServiceSyncTable<Player> PlayersTable => App.MobileService.GetSyncTable<Player>();
        private IMobileServiceSyncTable<GamePlayer> GamePlayersTable => App.MobileService.GetSyncTable<GamePlayer>();
        private IMobileServiceSyncTable<Venue> VenuesTable => App.MobileService.GetSyncTable<Venue>();
        private IMobileServiceSyncTable<VenueAvailabilityPeriod> VenueAvailPeriods => App.MobileService.GetSyncTable<VenueAvailabilityPeriod>();
        private IMobileServiceSyncTable<VenueFacilities> VenueFacilitiesTable => App.MobileService.GetSyncTable<VenueFacilities>();
        private IMobileServiceSyncTable<Facility> FacilitiesTable => App.MobileService.GetSyncTable<Facility>();

#else
        private IMobileServiceTable<Game> GamesTable => App.MobileService.GetTable<Game>();
        private IMobileServiceTable<Player> PlayersTable => App.MobileService.GetTable<Player>();
        private IMobileServiceTable<GamePlayer> GamePlayersTable => App.MobileService.GetTable<GamePlayer>();
        private IMobileServiceTable<Venue> VenuesTable => App.MobileService.GetTable<Venue>();
        private IMobileServiceTable<VenueAvailabilityPeriod> VenueAvailPeriods => App.MobileService.GetTable<VenueAvailabilityPeriod>();
        private IMobileServiceTable<VenueFacilities> VenueFacilitiesTable => App.MobileService.GetTable<VenueFacilities>();
        private IMobileServiceTable<Facility> FacilitiesTable => App.MobileService.GetTable<Facility>();
#endif
    }
}
