﻿using System;
using GlobalContracts.Enumerations;
using GlobalContracts.Interfaces;

namespace GolfScore.Domain
{
    public class Availability : DomainBase, IAvailability
    {
        public DateTime ClosingAt { get; set; }
        public DateTime OpemFrom { get; set; }
        public WeekDay WeekDay { get; set; }
    }
}