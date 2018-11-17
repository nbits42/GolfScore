using System.Collections.Generic;
using TeeScore.Domain;

namespace TeeScore.DTO
{
    public class GameDto
    {
        public Game Game  { get; set; }
        public Venue Venue { get; set; }
        public List<Player> Players { get; set; }
    }
}
