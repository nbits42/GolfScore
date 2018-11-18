using System;
using System.Collections.Generic;
using System.Text;
using TeeScore.Contracts;
using TeeScore.Domain;

namespace TeeScore.ViewModels
{
    public class VenueViewModel: MyViewModelBase
    {
        private Venue _venue;

        public VenueViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
        }

        /* =========================================== property: Venue ====================================== */
        /// <summary>
        /// Sets and gets the Venue property.
        /// </summary>
        public Venue Venue
        {
            get => _venue;
            set
            {
                if (value == _venue)
                {
                    return;
                }

                _venue = value;
                RaisePropertyChanged();
            }
        }


    }
}
