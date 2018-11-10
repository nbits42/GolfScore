using GlobalContracts.Interfaces;

namespace GolfScore.Domain
{
    public class VenueAvailabilityPeriod : DomainBase, IVenueAvailabilityPeriod
    {
        public string AvailabilityPeriodId { get; set; }
        public string VenueId { get; set; }
    }
}