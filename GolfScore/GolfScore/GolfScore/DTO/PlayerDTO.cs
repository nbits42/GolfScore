using System;
using GalaSoft.MvvmLight;
using GlobalContracts.Interfaces;

namespace TeeScore.DTO
{
    public class PlayerDto: ObservableObject, IPlayer
    {
        private string _name;
        private string _abbreviation;
        private string _id;
        private string _avatarUrl;
        private bool _isValid;

        public PlayerDto()
        {
            PropertyChanged += PlayerDto_PropertyChanged;
        }

        private void PlayerDto_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Abbreviation):
                case nameof(Name):
                    IsValid = !(string.IsNullOrEmpty(Abbreviation) || string.IsNullOrEmpty(Name));
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

        public DateTimeOffset? CreatedAt { get; set; }


        /* =========================================== property: Name ====================================== */
        public string ImageUrl { get; set; }

        /// <summary>
        /// Sets and gets the Name property.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name)
                {
                    return;
                }

                _name = value;
                RaisePropertyChanged();
            }
        }

        public string EmailAddress { get; set; }

        /* =========================================== property: Abbreviation ====================================== */
        /// <summary>
        /// Sets and gets the Abbreviation property.
        /// </summary>
        public string Abbreviation
        {
            get => _abbreviation;
            set
            {
                if (value == _abbreviation)
                {
                    return;
                }

                _abbreviation = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: AvatarUrl ====================================== */
        /// <summary>
        /// Sets and gets the AvatarUrl property.
        /// </summary>
        public string AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                if (value == _avatarUrl)
                {
                    return;
                }

                _avatarUrl = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: IsValid ====================================== */
        /// <summary>
        /// Sets and gets the IsValid property.
        /// </summary>
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (value == _isValid)
                {
                    return;
                }

                _isValid = value;
                RaisePropertyChanged();
            }
        }


        public override string ToString()
        {
            return Name;
        }

        public void Reset()
        {
            Name = string.Empty;
            Abbreviation = string.Empty;
        }
    }
}
