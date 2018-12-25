using System.Collections.Generic;
using System.Threading.Tasks;
using TeeScore.Domain;
using TeeScore.DTO;

namespace TeeScore.Contracts
{
    public interface IDataService
    {
        Task<List<Game>> GetGames(string playerId);
        Task<VenueDto> GetVenue(string venueId);
        Task<List<PlayerDto>> GetPlayersForGame(string gameId);
        Task InitializeAsync();
        Task<PlayerDto> GetPlayer(string playerId);
        Task<PlayerDto> SavePlayer(PlayerDto player, bool synchronize = false);
        Task<Game> SaveGame(Game game, bool synchronize = false);
        Task<VenueDto> SaveVenue(VenueDto venue, bool synchronize = false);
        Task<List<VenueDto>> GetVenues();
        Task<GamePlayer> SaveGamePlayer(GamePlayer gamePlayer, bool synchronize = false);
        Task<GamePlayer> GetGamePlayer(string gameId, string playerId);
        Task<List<PlayerDto>> GetKnownPlayers(string playerId);
        Task<PlayGameDto> GetGame(string gameId);
        Task<TeeDto> SaveTee(TeeDto tee, bool synchronize = false);
        Task<ScoreDto> SaveScore(ScoreDto teeScore, bool synchronize = false);
        Task<NewGameDto> GetNewGame(string gameId);
        Task DeleteGamePlayer(string gameId, string playerId);
        Task SyncAsync();
    }
}