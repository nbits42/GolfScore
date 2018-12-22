using System.Collections.Generic;
using TeeScore.Domain;

namespace TeeScore.DTO
{
    public class PlayGameDto
    {
        public Game Game  { get; set; } = new Game();
        public VenueDto Venue { get; set; }
        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
        public List<Tee> Tees { get; set; } = new List<Tee>();
        public List<Score>  Scores { get; set; } = new List<Score>();
    }
}
