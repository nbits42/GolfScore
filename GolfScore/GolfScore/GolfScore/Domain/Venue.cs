using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class Venue : DomainBase, IVenue
    {
        public string ImageUrl { get; set; }
        public double Lat { get; set; }
        public string Location { get; set; }
        public double Long { get; set; }
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}