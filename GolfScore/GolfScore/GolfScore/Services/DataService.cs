#define OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
        private const string dbVersion = "0.1";

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

            await App.MobileService.SyncContext.InitializeAsync(_store);
            await SyncAsync();
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
            var gameIds = await GamePlayersTable.Where(x => x.PlayerId == playerId).ToListAsync();
            var result = new List<Game>();
            foreach (var gameId in gameIds)
            {
                var game = await GamesTable.LookupAsync(gameId.GameId);
                if (game != null)
                {
                    result.Add(game);
                }
            }

            return result;
        }

        public async Task<Venue> GetVenue(string venueId)
        {
            return await VenuesTable.LookupAsync(venueId);
        }

        public async Task<List<Player>> GetPlayersForGame(string gameId)
        {
            var gamePlayers = await GamePlayersTable.Where(x => x.GameId == gameId).ToListAsync();
            var result = new List<Player>();
            foreach (var gamePlayer in gamePlayers)
            {
                result.Add(await PlayersTable.LookupAsync(gamePlayer.PlayerId));
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
                await App.MobileService.SyncContext.PushAsync();
                await GamesTable.PullAsync("AllGames", this.GamesTable.CreateQuery());
                await PlayersTable.PullAsync("AllPlayers", this.PlayersTable.CreateQuery());
                await VenuesTable.PullAsync("AllVenues", this.PlayersTable.CreateQuery());
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
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.",
                        error.TableName, error.Item["id"]);
                }
            }
        }

        public async Task<Player> GetPlayer(string myPlayerId)
        {
            return await PlayersTable.LookupAsync(myPlayerId);
        }

        public async Task<Player> SavePlayer(Player player)
        {
            //if (player.IsNew)
            //{
            //    player.Id = NewId();
            //    await PlayersTable.InsertAsync(player);
            //}
            //else
            //{
            //    await PlayersTable.UpdateAsync(player);
            //}

            //await SyncAsync();
            return await SaveAsync<Player>(player);
        }

        public async Task<Game> SaveGame(Game game)
        {
            return await SaveAsync<Game>(game);
        }

        public async Task<Venue> SaveVenue(Venue venue)
        {
            return await SaveAsync<Venue>(venue);
            //if (venue.IsNew)
            //{
            //    venue.Id = NewId();
            //    await VenuesTable.InsertAsync(venue);
            //}
            //else
            //{
            //    await VenuesTable.UpdateAsync(venue);
            //}

            //return await VenuesTable.LookupAsync(venue.Id);
        }

        private async Task<T> SaveAsync<T>(DomainBase entity) where T : DomainBase
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
                    await table.InsertAsync((T)entity);
                }
                else
                {
                    action = "update";
                    await table.UpdateAsync((T)entity);
                }
                action = "sync";
                await SyncAsync();
                action = "lookup";

                return await table.LookupAsync(entity.Id);
            }
            catch (Exception e)
            {

                Debug.WriteLine($"Error executing {action} operation. Item: {typeof(T).Name} ({entity.Id}). Operation discarded. Error: {e.Message}");
                throw;
            }
        }

        public async Task<List<Venue>> GetVenues()
        {
            return await VenuesTable.ToListAsync();
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
