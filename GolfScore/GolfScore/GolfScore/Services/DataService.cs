#define OFFLINE_SYNC_ENABLED
using AutoMapper;
using GlobalContracts.Enumerations;
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
using TeeScore.DTO;
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
            _store.DefineTable<Tee>();
            _store.DefineTable<Score>();

#if OFFLINE_SYNC_ENABLED

            await App.MobileService.SyncContext.InitializeAsync(_store).ConfigureAwait(false);

            await CleanupGames();
            //var players = await PlayersTable.ToListAsync();
            //if (players.Count == 0)
            //{
            await SyncAsync().ConfigureAwait(false);
            //}
#endif

            MessagingCenter.Send(this, ServiceMessage.DataServiceInitialized);
        }

        private async Task CleanupGames()
        {
            var now = DateTimeOffset.Now.AddDays(-2);
            var games = await GamesTable.Where(x => x.CreatedAt < now).ToListAsync().ConfigureAwait(false);
            foreach (var game in games)
            {
                if ((int)game.GameStatus < (int)GameStatus.Started)
                {
                    var gamePlayers = await GamePlayersTable.Where(x => x.GameId == game.Id).ToListAsync().ConfigureAwait(false);
                    foreach (var gamePlayer in gamePlayers)
                    {
                        await GamePlayersTable.DeleteAsync(gamePlayer).ConfigureAwait(false);
                    }
                    await GamesTable.DeleteAsync(game).ConfigureAwait(false);
                }
            }
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
            var games = await GamesTable.ToListAsync().ConfigureAwait(false);
            var result = new List<Game>();
            foreach (var gamePlayer in gamePlayers)
            {
                var game = games.FirstOrDefault(x => x.Id == gamePlayer.GameId);
                if (game != null)
                {
                    result.Add(game);
                }
            }

            return result;
        }

        public async Task<VenueDto> GetVenue(string venueId)
        {
            if (string.IsNullOrEmpty(venueId))
            {
                return null;
            }
            return Mapper.Map<VenueDto>(await VenuesTable.LookupAsync(venueId).ConfigureAwait(false));
        }

        public async Task<List<PlayerDto>> GetPlayersForGame(string gameId)
        {
            var gamePlayers = await GamePlayersTable.Where(x => x.GameId == gameId).ToListAsync().ConfigureAwait(false);
            var result = new List<PlayerDto>();
            foreach (var gamePlayer in gamePlayers)
            {
                result.Add(await GetPlayer(gamePlayer.PlayerId));
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
                await TeesTable.PullAsync("AllTees", this.TeesTable.CreateQuery()).ConfigureAwait(false);
                await ScoresTable.PullAsync("AllScores", this.ScoresTable.CreateQuery()).ConfigureAwait(false);
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

        public async Task<Game> GetGame(int invitationNumber)
        {
            await SyncAsync();
            var games = await GamesTable.Where(x => x.InvitationNumber == invitationNumber).ToListAsync();
            return games.FirstOrDefault();
        }

        public async Task<Game> GetGame(string gameId)
        {
            await SyncAsync();
            return await GamesTable.LookupAsync(gameId);
        }

        public async Task<PlayerDto> GetPlayer(string myPlayerId)
        {
            return Mapper.Map<PlayerDto>(await PlayersTable.LookupAsync(myPlayerId).ConfigureAwait(false));
        }

        public async Task<PlayerDto> SavePlayer(PlayerDto player, bool synchronize = false)
        {
            var saved = await SaveAsync<Player>(Mapper.Map<Player>(player), synchronize).ConfigureAwait(false);
            return Mapper.Map<PlayerDto>(saved);
        }

        public async Task<Game> SaveGame(Game game, bool synchronize = false)
        {
            return await SaveAsync<Game>(game, synchronize).ConfigureAwait(false);
        }

        public async Task<VenueDto> SaveVenue(VenueDto venue, bool synchronize = false)
        {
            var saved = await SaveAsync<Venue>(Mapper.Map<Venue>(venue), synchronize).ConfigureAwait(false);
            return Mapper.Map<VenueDto>(saved);
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
                    await SyncAsync().ConfigureAwait(false);
                }
                action = "lookup";

                return (T)entity;
            }
            catch (Exception e)
            {
                var msg = $"Error executing {action} operation. Item: {typeof(T).Name} ({entity.Id}). Operation discarded. ";
                ErrorReportingService.ReportError(this, e, msg);
                //Debug.WriteLine($"{msg}. Error: {e.Message}");
                throw;
            }
        }

        public async Task<List<VenueDto>> GetVenues()
        {
            return Mapper.Map<List<Venue>, List<VenueDto>>(await VenuesTable.ToListAsync().ConfigureAwait(false));
        }

        public async Task<GamePlayer> SaveGamePlayer(GamePlayer gamePlayer, bool synchronize = false)
        {
            return await SaveAsync<GamePlayer>(gamePlayer, synchronize);
        }

        public async Task<GamePlayer> GetGamePlayer(string gameId, string playerId)
        {
            var result = await GamePlayersTable.Where(x => x.GameId == gameId && x.PlayerId == playerId).ToListAsync().ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public async Task<List<PlayerDto>> GetKnownPlayers(string playerId)
        {
            var myGames = await GetGames(playerId);
            var knownPlayers = new List<PlayerDto>();
            foreach (var game in myGames)
            {
                var players = (await GetPlayersForGame(game.Id).ConfigureAwait(false)).Where(x => x.Id != playerId);
                foreach (var player in players)
                {
                    if (!knownPlayers.Exists(x => x.Id == player.Id))
                    {
                        knownPlayers.Add(Mapper.Map<PlayerDto>(player));
                    }
                }
            }

            return knownPlayers;
        }

        public async Task<PlayGameDto> GetPlayGame(string gameId)
        {
            var result = new PlayGameDto
            {
                Game = await GamesTable.LookupAsync(gameId).ConfigureAwait(false),
                Players = (await GetPlayersForGame(gameId).ConfigureAwait(false)).OrderBy(x => x.Abbreviation).ToList(),
                Tees = Mapper.Map<List<Tee>, List<TeeDto>>(await TeesTable.Where(x => x.GameId == gameId).OrderBy(x => x.Number).ToListAsync().ConfigureAwait(false)),
                Scores = Mapper.Map<List<Score>, List<ScoreDto>>(await ScoresTable.Where(x => x.GameId == gameId).ToListAsync().ConfigureAwait(false)),
            };
            result.Venue = await GetVenue(result.Game.VenueId).ConfigureAwait(false);
            return result;
        }

        public async Task<NewGameDto> GetNewGame(string gameId)
        {
            var result = new NewGameDto
            {
                Game = await GamesTable.LookupAsync(gameId).ConfigureAwait(false),
                Players = await GetPlayersForGame(gameId).ConfigureAwait(false),
            };
            result.Venue = await GetVenue(result.Game.VenueId).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteGamePlayer(string gameId, string playerId, bool synchronize = false)
        {
            var gamePlayer = await GetGamePlayer(gameId, playerId).ConfigureAwait(false);
            if (gamePlayer != null)
            {
                await GamePlayersTable.DeleteAsync(gamePlayer).ConfigureAwait(false);
                await SyncAsync();
            }
        }

        public void SetGame(string gameId)
        {
            CurrentGameId = gameId;
        }

        public string CurrentGameId { get; private set; }
        public async Task<TeeDto> SaveTee(TeeDto tee, bool synchronize = false)
        {
            var saved = await SaveAsync<Tee>(Mapper.Map<Tee>(tee), synchronize);
            return Mapper.Map<TeeDto>(saved);
        }

        public async Task<ScoreDto> SaveScore(ScoreDto teeScore, bool synchronize = false)
        {
            var saved = await SaveAsync<Score>(Mapper.Map<Score>(teeScore), synchronize);
            return Mapper.Map<ScoreDto>(saved);
        }


#if OFFLINE_SYNC_ENABLED
        private IMobileServiceSyncTable<Game> GamesTable => App.MobileService.GetSyncTable<Game>();
        private IMobileServiceSyncTable<Player> PlayersTable => App.MobileService.GetSyncTable<Player>();
        private IMobileServiceSyncTable<GamePlayer> GamePlayersTable => App.MobileService.GetSyncTable<GamePlayer>();
        private IMobileServiceSyncTable<Venue> VenuesTable => App.MobileService.GetSyncTable<Venue>();
        private IMobileServiceSyncTable<VenueAvailabilityPeriod> VenueAvailPeriods => App.MobileService.GetSyncTable<VenueAvailabilityPeriod>();
        private IMobileServiceSyncTable<VenueFacilities> VenueFacilitiesTable => App.MobileService.GetSyncTable<VenueFacilities>();
        private IMobileServiceSyncTable<Facility> FacilitiesTable => App.MobileService.GetSyncTable<Facility>();
        private IMobileServiceSyncTable<Tee> TeesTable => App.MobileService.GetSyncTable<Tee>();
        private IMobileServiceSyncTable<Score> ScoresTable => App.MobileService.GetSyncTable<Score>();

#else
        private IMobileServiceTable<Game> GamesTable => App.MobileService.GetTable<Game>();
        private IMobileServiceTable<Player> PlayersTable => App.MobileService.GetTable<Player>();
        private IMobileServiceTable<GamePlayer> GamePlayersTable => App.MobileService.GetTable<GamePlayer>();
        private IMobileServiceTable<Venue> VenuesTable => App.MobileService.GetTable<Venue>();
        private IMobileServiceTable<VenueAvailabilityPeriod> VenueAvailPeriods => App.MobileService.GetTable<VenueAvailabilityPeriod>();
        private IMobileServiceTable<VenueFacilities> VenueFacilitiesTable => App.MobileService.GetTable<VenueFacilities>();
        private IMobileServiceTable<Facility> FacilitiesTable => App.MobileService.GetTable<Facility>();
        private IMobileServiceTable<Tee> TeesTable => App.MobileService.GetTable<Tee>();
        private IMobileServiceTable<Score> ScoresTable => App.MobileService.GetTable<Score>();
#endif
    }
}
