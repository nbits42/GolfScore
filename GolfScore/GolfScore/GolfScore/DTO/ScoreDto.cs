using System;
using GalaSoft.MvvmLight;
using GlobalContracts.Interfaces;

namespace TeeScore.DTO
{
    public class ScoreDto : ObservableObject, IScore
    {
        private string _id;
        private string _gameId;
        private string _teeId;
        private string _playerId;
        private int _putts;
        private string _playerAbbreviation;
        private PlayerDto _player;
        private string _teeNumber;

        public event EventHandler ScoreChanged;

        public ScoreDto()
        {
            PropertyChanged += ScoreDto_PropertyChanged;
        }

        private void ScoreDto_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Putts):
                    OnScoreChanged();
                    break;
            }
        }

        /* =========================================== property: Id ====================================== */
        /// <summary>
        /// Sets and gets the Id property.
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                if (value == _id)
                {
                    return;
                }

                _id = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: GameId ====================================== */
        /// <summary>
        /// Sets and gets the GameId property.
        /// </summary>
        public string GameId
        {
            get => _gameId;
            set
            {
                if (value == _gameId)
                {
                    return;
                }

                _gameId = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: TeeId ====================================== */
        /// <summary>
        /// Sets and gets the TeeId property.
        /// </summary>
        public string TeeId
        {
            get => _teeId;
            set
            {
                if (value == _teeId)
                {
                    return;
                }

                _teeId = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: PlayerId ====================================== */
        /// <summary>
        /// Sets and gets the PlayerId property.
        /// </summary>
        public string PlayerId
        {
            get => _playerId;
            set
            {
                if (value == _playerId)
                {
                    return;
                }

                _playerId = value;
                RaisePropertyChanged();
            }
        }


/* =========================================== property: Putts ====================================== */
        /// <summary>
        /// Sets and gets the Putts property.
        /// </summary>
        public int Putts
        {
            get => _putts;
            set
            {
                if (value == _putts)
                {
                    return;
                }

                _putts = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: Player ====================================== */
        /// <summary>
        /// Sets and gets the Player property.
        /// </summary>
        public PlayerDto Player
        {
            get => _player;
            set
            {
                if (value == _player)
                {
                    return;
                }

                _player = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== property: PlayerAbbreviation ====================================== */
        /// <summary>
        /// Sets and gets the PlayerAbbreviation property.
        /// </summary>
        public string PlayerAbbreviation
        {
            get => _playerAbbreviation;
            set
            {
                if (value == _playerAbbreviation)
                {
                    return;
                }

                _playerAbbreviation = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: TeeNumber ====================================== */
        /// <summary>
        /// Sets and gets the TeeNumber property.
        /// </summary>
        public string  TeeNumber
        {
            get => _teeNumber;
            set
            {
                if (value == _teeNumber)
                {
                    return;
                }

                _teeNumber = value;
                RaisePropertyChanged();
            }
        }


        protected virtual void OnScoreChanged()
        {
            ScoreChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}