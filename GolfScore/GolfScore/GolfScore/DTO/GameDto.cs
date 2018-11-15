using System;
using System.Collections.Generic;
using System.Text;
using GolfScore.Domain;

namespace GolfScore.DTO
{
    public class GameDto
    {
        public Game Game  { get; set; }
        public Venue Venue { get; set; }
        public List<Player> Players { get; set; }
    }
}
