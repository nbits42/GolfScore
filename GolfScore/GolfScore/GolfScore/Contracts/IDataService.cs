using System.Collections.Generic;
using System.Threading.Tasks;
using GolfScore.Domain;

namespace GolfScore.Contracts
{
    public interface IDataService
    {
        Task<Game> NewGame(Game item);
        Task<List<Game>> GetGames(string playerId);
        Task<Player> NewPlayer(Player newPlayer);
        Task<Venue> GetVenue(string venueId);
        Task<List<Player>> GetPlayersForGame(string gameId);
        Task InitializeAsync();
        Task<Player> GetPlayer(string myPlayerId);
        Task SavePlayer(Player myPlayer);
    }
}