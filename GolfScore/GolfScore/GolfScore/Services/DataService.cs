#define OFFLINE_SYNC_ENABLED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalContracts.Enumerations;
using GolfScore.Contracts;
using Microsoft.WindowsAzure.MobileServices;
using GolfScore.Domain;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Xamarin.Forms;

namespace GolfScore.Services
{
    public class DataService : IDataService
    {
        public DataService()
        {
            Task.Run(() => InitializeAsync().ConfigureAwait(false));
        }

        private async Task InitializeAsync()
        {
#if OFFLINE_SYNC_ENABLED
            var path = DependencyService.Get<IDatabaseConnection>().DbConnection("0.0");
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Game>();
            store.DefineTable<Player>();
            store.DefineTable<GamePlayer>();
            store.DefineTable<Venue>();
            store.DefineTable<VenueAvailabilityPeriod>();
            store.DefineTable<VenueFacilities>();
            store.DefineTable<Facility>();
            store.DefineTable<Availability>();
            store.DefineTable<AvailabilityPeriod>();
            await App.MobileService.SyncContext.InitializeAsync(store);
#endif
        }

        public async Task<Game> NewGame(Game item)
        {
            item.Id = NewId();
            await GamesTable.InsertAsync(item);
            return await GamesTable.LookupAsync(item.Id);
        }

        private static string NewId()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<List<Game>> GetGames(string playerId)
        {
            var gameIds = await GamePlayersTable.Where(x => x.PlayerId == playerId).ToListAsync();
            return await GamesTable.Where(x => gameIds.Any(y=>y.GameId == x.Id)).ToListAsync();
        }

        public async Task<Player> NewPlayer(Player newPlayer)
        {
            newPlayer.Id = NewId();
            await PlayersTable.InsertAsync(newPlayer);

            return await PlayersTable.LookupAsync(newPlayer.Id);

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
