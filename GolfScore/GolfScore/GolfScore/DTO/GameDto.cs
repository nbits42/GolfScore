﻿using System.Collections.Generic;
using TeeScore.Domain;

namespace TeeScore.DTO
{
    public class GameDto
    {
        public Game Game  { get; set; } = new Game();
        public Venue Venue { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Tee> Tees { get; set; } = new List<Tee>();
        public List<Score>  Scores { get; set; } = new List<Score>();
    }
}
