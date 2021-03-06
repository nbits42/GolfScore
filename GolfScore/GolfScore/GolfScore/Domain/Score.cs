﻿using System;
using System.Collections.Generic;
using System.Text;
using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class Score: DomainBase, IScore
    {
        public string GameId { get; set; }
        public string TeeId { get; set; }
        public string PlayerId { get; set; }
        public string PlayerAbbreviation { get; set; }
        public int Putts { get; set; } = 0;
    }
}
