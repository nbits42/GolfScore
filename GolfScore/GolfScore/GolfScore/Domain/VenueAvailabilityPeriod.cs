using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class VenueAvailabilityPeriod : DomainBase, IVenueAvailabilityPeriod
    {
        public string AvailabilityPeriodId { get; set; }
        public string VenueId { get; set; }
    }
}