using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class Venue : DomainBase, IVenue
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


    }
}