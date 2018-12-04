using System.Collections.Generic;
using System.Threading.Tasks;
using TeeScore.Domain;

namespace TeeScore.Contracts
{
    public interface IDataService
    {
        Task<List<Game>> GetGames(string playerId);
        Task<Venue> GetVenue(string venueId);
        Task<List<Player>> GetPlayersForGame(string gameId);
        Task InitializeAsync();
        Task<Player> GetPlayer(string playerId);
        Task<Player> SavePlayer(Player player);
        Task<Game> SaveGame(Game game);
        Task<Venue> SaveVenue(Venue venue);
        Task<List<Venue>> GetVenues();
        Task<GamePlayer> SaveGamePlayer(GamePlayer gamePlayer);
    }
}