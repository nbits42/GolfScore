using GlobalContracts.Enumerations;
using GlobalContracts.Interfaces;
using System;

namespace TeeScore.Domain
{
    public class Game : DomainBase, IGame
    {
        #region Private Fields      =====================================================
        private int _teeCount;
        private int _connectedPlayersCount;
        private GameType _gameType;
        private int _invitedPlayersCount;
        private int _invitationNumber;
        private int _startTee;
        private int _currentTee;

        #endregion

        #region Properties          =====================================================

        public DateTime Date { get; set; }
        public GameStatus GameStatus { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public string VenueId { get; set; }

        /* =========================================== property: GameType ====================================== */
        /// <summary>
        /// Sets and gets the GameType property.
        /// </summary>
        public GameType GameType
        {
            get => _gameType;
            set
            {
                if (value == _gameType)
                {
                    return;
                }

                _gameType = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: InvitedPlayersCount ====================================== */
        /// <summary>
        /// Sets and gets the InvitedPlayersCount property.
        /// </summary>
        public int InvitedPlayersCount
        {
            get => _invitedPlayersCount;
            set
            {
                if (value == _invitedPlayersCount)
                {
                    return;
                }

                _invitedPlayersCount = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: ConnectedPlayersCount ====================================== */
        /// <summary>
        /// Sets and gets the ConnectedPlayersCount property.
        /// </summary>
        public int ConnectedPlayersCount
        {
            get => _connectedPlayersCount;
            set
            {
                if (value == _connectedPlayersCount)
                {
                    return;
                }

                _connectedPlayersCount = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: TeeCount ====================================== */
        /// <summary>
        /// Sets and gets the TeeCount property.
        /// </summary>
        public int TeeCount
        {
            get => _teeCount;
            set
            {
                if (value == _teeCount)
                {
                    return;
                }

                _teeCount = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: StartTee ====================================== */
        /// <summary>
        /// Sets and gets the StartTee property.
        /// </summary>
        public int StartTee
        {
            get => _startTee;
            set
            {
                if (value == _startTee)
                {
                    return;
                }

                _startTee = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: CurrentTee ====================================== */
        /// <summary>
        /// Sets and gets the CurrentTee property.
        /// </summary>
        public int CurrentTee
        {
            get => _currentTee;
            set
            {
                if (value == _currentTee)
                {
                    return;
                }

                _currentTee = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== property: InvitationNumber ====================================== */
        /// <summary>
        /// Sets and gets the InvitationNumber property.
        /// </summary>
        public int InvitationNumber
        {
            get => _invitationNumber;
            set
            {
                if (value == _invitationNumber)
                {
                    return;
                }

                _invitationNumber = value;
                RaisePropertyChanged();
            }
        }

        #endregion


    }
}
