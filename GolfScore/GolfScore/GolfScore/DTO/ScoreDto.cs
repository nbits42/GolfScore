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

    }
}