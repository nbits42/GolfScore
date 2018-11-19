using GlobalContracts.Interfaces;
using System.ComponentModel;

namespace TeeScore.Domain
{
    public class Venue : DomainBase, IVenue, IDataErrorInfo
    {
        private string _name;
        private string _location;
        public string ImageUrl { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string ThumbnailUrl { get; set; }

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

        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                var msg = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrEmpty(Name))
                            msg = Translations.Messages.NameIsRequired;
                        break;
                    case nameof(Location):
                        if (string.IsNullOrEmpty(Location))
                            msg = Translations.Messages.LocationIsRequired;
                        break;
                }
                return msg;
            }
        }
    }
}