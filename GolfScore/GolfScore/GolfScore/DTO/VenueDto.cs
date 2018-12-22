using System;
using GalaSoft.MvvmLight;
using GlobalContracts.Interfaces;
using TeeScore.Validation;

namespace TeeScore.DTO
{
    public class VenueDto: ObservableObject, IVenue
    {
        private string _ownerId;
        private string _id;
        private DateTimeOffset? _createdAt;
        private string _imageUrl;
        private double _lat;
        private double _long;
        private string _location;
        private string _name;
        private string _thumbnailUrl;
        private bool _isValid;

        public VenueDto()
        {
            PropertyChanged += VenueDto_PropertyChanged;
        }

        private void VenueDto_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Name):
                case nameof(Location):
                    Validate();
                    break;
            }
        }

        private void Validate()
        {
            IsValid = !(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Location));
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

        /* =========================================== property: CreatedAt ====================================== */
        /// <summary>
        /// Sets and gets the CreatedAt property.
        /// </summary>
        public DateTimeOffset? CreatedAt
        {
            get => _createdAt;
            set
            {
                if (value == _createdAt)
                {
                    return;
                }

                _createdAt = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: ImageUrl ====================================== */
        /// <summary>
        /// Sets and gets the ImageUrl property.
        /// </summary>
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (value == _imageUrl)
                {
                    return;
                }

                _imageUrl = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: Lat ====================================== */
        /// <summary>
        /// Sets and gets the Lat property.
        /// </summary>
        public double Lat
        {
            get => _lat;
            set
            {
                if (Math.Abs(value - _lat) < 0.1)
                {
                    return;
                }

                _lat = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: Long ====================================== */
        /// <summary>
        /// Sets and gets the Long property.
        /// </summary>
        public double Long
        {
            get => _long;
            set
            {
                if (Math.Abs(value - _long) < 0.1)
                {
                    return;
                }

                _long = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: Location ====================================== */
        /// <summary>
        /// Sets and gets the Location property.
        /// </summary>
        public string Location
        {
            get => _location;
            set
            {
                if (value == _location)
                {
                    return;
                }

                _location = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: Name ====================================== */
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

/* =========================================== property: ThumbnailUrl ====================================== */
        /// <summary>
        /// Sets and gets the ThumbnailUrl property.
        /// </summary>
        public string ThumbnailUrl
        {
            get => _thumbnailUrl;
            set
            {
                if (value == _thumbnailUrl)
                {
                    return;
                }

                _thumbnailUrl = value;
                RaisePropertyChanged();
            }
        }


/* =========================================== property: OwnerId ====================================== */
        /// <summary>
        /// Sets and gets the OwnerId property.
        /// </summary>
        public string OwnerId
        {
            get => _ownerId;
            set
            {
                if (value == _ownerId)
                {
                    return;
                }

                _ownerId = value;
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



    }
}
