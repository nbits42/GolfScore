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
        Task<Player> SavePlayer(Player player, bool synchronize = false);
        Task<Game> SaveGame(Game game, bool synchronize = false);
        Task<Venue> SaveVenue(Venue venue, bool synchronize = false);
        Task<List<Venue>> GetVenues();
        Task<GamePlayer> SaveGamePlayer(GamePlayer gamePlayer, bool synchronize = false);
        Task<GamePlayer> GetGamePlayer(string gameId, string playerId);
        Task<List<Player>> GetKnownPlayers(string playerId);
    }
}