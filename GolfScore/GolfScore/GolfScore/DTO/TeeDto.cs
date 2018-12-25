using System;
using GalaSoft.MvvmLight;
using GlobalContracts.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeeScore.Domain;

namespace TeeScore.DTO
{
    public class TeeDto : ObservableObject, ITee
    {
        private string _id;
        private DateTime _finished = DomainBase.EmptyDate;
        private DateTime _started = DomainBase.EmptyDate;
        private string _number;
        private string _gameId;

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

/* =========================================== property: Finished ====================================== */
        /// <summary>
        /// Sets and gets the Finished property.
        /// </summary>
        public DateTime Finished
        {
            get => _finished;
            set
            {
                if (value == _finished)
                {
                    return;
                }

                _finished = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: Started ====================================== */
        /// <summary>
        /// Sets and gets the Started property.
        /// </summary>
        public DateTime Started
        {
            get => _started;
            set
            {
                if (value == _started)
                {
                    return;
                }

                _started = value;
                RaisePropertyChanged();
            }
        }


        /* =========================================== property: Number ====================================== */
        /// <summary>
        /// Sets and gets the Number property.
        /// </summary>
        public string Number
        {
            get => _number;
            set
            {
                if (value == _number)
                {
                    return;
                }

                _number = value;
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

        public ObservableCollection<ScoreDto> Scores { get; set; } = new ObservableCollection<ScoreDto>();
    }
}
