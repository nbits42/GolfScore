using System.Threading.Tasks;
using Syncfusion.SfNumericUpDown.XForms;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.Helpers;
using TeeScore.Validation;

namespace TeeScore.ViewModels
{
    public class VenueViewModel : ValidatableViewModelBase
    {
        private Venue _venue = new Venue();
        private ValidatableObject<string> _name = new ValidatableObject<string>();
        private ValidatableObject<string> _location = new ValidatableObject<string>();

        public VenueViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
        }

        protected override void AddValidations()
        {
            _name.Validations.Add(new IsNotNullOrEmptyRule<string>(Translations.Messages.NameIsRequired));
            _location.Validations.Add(new IsNotNullOrEmptyRule<string>(Translations.Messages.NameIsRequired));
        }

        public override bool Validate()
        {
            var isValidName = ValidateNameCommand();
            var isValidLocation = ValidateLocationCommand();
            return isValidName && isValidLocation;
        }

        public bool ValidateLocationCommand()
        {
            return _location.Validate();
        }

        public bool ValidateNameCommand()
        {
            return _name.Validate();
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


        /* =========================================== validatable property: Name ====================================== */

        public ValidatableObject<string> Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /* =========================================== validatable property: Location ====================================== */

        public ValidatableObject<string> Location
        {
            get => _location;
            set
            {
                _location = value;
                RaisePropertyChanged(() => Location);
            }
        }


        public async Task LoadAsync(string venueId)
        {
            if (string.IsNullOrEmpty(venueId))
            {
                Venue = new Venue {OwnerId = Settings.MyPlayerId};
            }
            else
            {
                Venue = await DataService.GetVenue(venueId);
            }

            Name.Value = Venue.Name;
            Location.Value = Venue.Location;
            IsValid = Validate();
        }

        public async Task SaveAsync()
        {
            Venue.Name = Name.Value;
            Venue.Location = Location.Value;
            Venue.Lat = 0;
            Venue.Long = 0;
            Venue = await DataService.SaveVenue(Venue);
        }
    }
}
