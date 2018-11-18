using System.Collections.Generic;
using TeeScore.Domain;

namespace TeeScore.DTO
{
    public class GameDto
    {
        public Game Game  { get; set; } = new Game();
        public Venue Venue { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
