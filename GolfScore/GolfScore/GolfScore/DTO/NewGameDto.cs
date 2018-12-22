using System.Collections.Generic;
using TeeScore.Domain;

namespace TeeScore.DTO
{
    public class NewGameDto
    {
        public Game Game  { get; set; } = new Game();
        public VenueDto Venue { get; set; }
        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
    }
}
