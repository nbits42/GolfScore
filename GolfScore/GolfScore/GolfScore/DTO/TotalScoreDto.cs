using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace TeeScore.DTO
{
    public class TotalScoreDto: ObservableObject
    {
        private int _totalPutts;
        private string _playerId;

        /* =========================================== property: TotalPutts ====================================== */
        /// <summary>
        /// Sets and gets the TotalPutts property.
        /// </summary>
        public int TotalPutts
        {
            get => _totalPutts;
            set
            {
                if (value == _totalPutts)
                {
                    return;
                }

                _totalPutts = value;
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

    }
}
