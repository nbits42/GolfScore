using System.Collections.Generic;
using System.Threading.Tasks;
using GolfScore.Domain;

namespace GolfScore.Services
{
    public interface IDataService
    {
        Task<Game> NewGame(Game item);
        Task<List<Game>> GetGames(string playerId);
        Task<Player> NewPlayer(Player newPlayer);
    }
}