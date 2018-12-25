using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using Syncfusion.SfNumericUpDown.XForms;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;
using TeeScore.Validation;
using INavigationService = TeeScore.Contracts.INavigationService;

namespace TeeScore.ViewModels
{
    public class VenueViewModel : MyViewModelBase
    {
        private readonly IModalDialogService _dialogService;
        private VenueDto _venue = new VenueDto();
        private ObservableCollection<VenueDto> _venues;

        public VenueViewModel(IDataService dataService, INavigationService navigationService, IModalDialogService dialogService) : base(dataService, navigationService)
        {
            _dialogService = dialogService;
        }

        /* =========================================== property: Venue ====================================== */
        /// <summary>
        /// Sets and gets the Venue property.
        /// </summary>
        public VenueDto Venue
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

        /* =========================================== property: Venues ====================================== */
        /// <summary>
        /// Sets and gets the Venues property.
        /// </summary>
        public ObservableCollection<VenueDto> Venues
        {
            get => _venues;
            set
            {
                if (value == _venues)
                {
                    return;
                }

                _venues = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadAsync(string venueId)
        {
            if (string.IsNullOrEmpty(venueId))
            {
                Venue = new VenueDto {OwnerId = Settings.MyPlayerId};
            }
            else
            {
                Venue = await DataService.GetVenue(venueId);
            }
        }

       
        public async Task<bool> ValidateAndSave()
        {
            var valid = Venue.IsValid;

            if (valid) 
            {
                if (Venues.Any(x=>x.Name.Equals(Venue.Name, StringComparison.OrdinalIgnoreCase) && x.Location.Equals(Venue.Location, StringComparison.OrdinalIgnoreCase)))
                {
                    _dialogService.ShowError("This Venue is already in your venues list", "Validation error");
                    return false;
                }
            }

            if (valid)
            {
                Venue = await DataService.SaveVenue(Venue, false);
                Venues.Insert(0, Venue);
            }

            return valid;
        }
    }
}
