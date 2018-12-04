﻿using GlobalContracts.Enumerations;
using GlobalContracts.Interfaces;
using System;

namespace TeeScore.Domain
{
    public class Game : DomainBase, IGame
    {
        #region Properties          =====================================================

        public DateTime Date { get; set; } = DateTime.Now;
        public GameStatus GameStatus { get; set; }
        public DateTime? FinishedAt { get; set; } = EmptyDate;
        public DateTime? StartedAt { get; set; } = EmptyDate;
        public string VenueId { get; set; }
        public GameType GameType { get; set; }
        public int InvitedPlayersCount { get; set; }
        public int ConnectedPlayersCount { get; set; }
        public int TeeCount { get; set; }
        public int StartTee { get; set; }
        public int CurrentTee { get; set; }
        public int InvitationNumber { get; set; }
        public PlayerSelection PlayerSelection { get; set; }
        #endregion


    }
}
