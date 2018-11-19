using System;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.Validation;

namespace TeeScore.ViewModels
{
    public class VenueViewModel : ValidatableViewModelBase
    {
        private Venue _venue = new Venue();
        private bool _validated;
        private ValidatableObject<string> _name;
        private ValidatableObject<string> _location;

        public VenueViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
        }

        protected override void AddValidations()
        {
            _name.Validations.Add(new IsNotNullOrEmptyRule<string>(Translations.Messages.NameIsRequired));
            _location.Validations.Add(new IsNotNullOrEmptyRule<string>(Translations.Messages.NameIsRequired));
        }

        protected override bool Validate()
        {
            var isValidName = ValidateName();
            var isValidLocation = ValidateLocation();
            return isValidName && isValidLocation;
        }

        private bool ValidateLocation()
        {
            return _location.Validate();
        }

        private bool ValidateName()
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




    }
}
