using GlobalContracts.Interfaces;

namespace GolfScore.Domain
{
    public class VenueFacilities : DomainBase, IVenueFacilities
    {
        public string FacilityId { get; set; }
        public string VenueId { get; set; }
    }
}