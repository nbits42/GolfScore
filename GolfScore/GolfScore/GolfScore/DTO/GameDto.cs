using System.Collections.Generic;
using TeeScore.Domain;

namespace TeeScore.DTO
{
    public class PlayGameDto
    {
        public Game Game  { get; set; } = new Game();
        public VenueDto Venue { get; set; }
        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
        public List<TeeDto> Tees { get; set; } = new List<TeeDto>();
        public List<ScoreDto>  Scores { get; set; } = new List<ScoreDto>();
    }
}
