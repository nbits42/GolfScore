using System;
using System.Collections.Generic;
using System.Text;
using GlobalContracts.Enumerations;
using GlobalContracts.Interfaces;

namespace GolfScore.Domain
{
    public class Game: DomainBase, IGame
    {
        public DateTime Date { get; set; }
        public GameStatus GameStatus { get; set; }
        public GameType GameType { get; set; }
        public int InvitedPlayersCount { get; set; }
        public int ConnectedPlayersCount { get; set; }
        public int TeeCount { get; set; }
        public string VenueId { get; set; }
        public int InvitationNumber { get; set; }
    }
}
