using GlobalContracts.Enumerations;
using GlobalContracts.Interfaces;

namespace GolfScore.Domain
{
    public class Facility : DomainBase, IFacility
    {
        public string Description { get; set; }
        public FacilityType FacilityType { get; set; }
    }
}