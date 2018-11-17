using GlobalContracts.Enumerations;
using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class Facility : DomainBase, IFacility
    {
        public string Description { get; set; }
        public FacilityType FacilityType { get; set; }
    }
}