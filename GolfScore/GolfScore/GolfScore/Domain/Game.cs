using GlobalContracts.Enumerations;
using GlobalContracts.Interfaces;
using System;
using System.Linq;
using System.Text;
using TeeScore.Services;

namespace TeeScore.Domain
{
    public class Game : DomainBase, IGame
    {
        #region Properties          =====================================================

        public DateTime GameDate { get; set; } = DateTime.Now;
        public GameStatus GameStatus { get; set; }
        public DateTime FinishedAt { get; set; } = EmptyDate;
        public DateTime StartedAt { get; set; } = EmptyDate;
        public string VenueId { get; set; }
        public GameType GameType { get; set; }
        public int InvitedPlayersCount { get; set; }
        public int ConnectedPlayersCount { get; set; }
        public int TeeCount { get; set; }
        public int StartTee { get; set; }
        public int CurrentTee { get; set; }
        public int InvitationNumber { get; set; }
        public PlayerSelection PlayerSelection { get; set; }
        public string VenueName { get; set; }
        public string PlayerNames { get; set; }
        public string ScoresJson { get; set; }

        public string GameTypeName => TranslationService.Translate($"GameType_{GameType}");
        public string Shortcut
        {
            get
            {
                var parts = VenueName.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var shortcut = new StringBuilder();
                foreach (var part in parts.Take(2))
                {
                    shortcut.Append(part[0]);
                }
                return shortcut.ToString();
            }
        }

        #endregion


    }
}
