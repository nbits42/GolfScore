using System;
using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class AvailabilityPeriod : DomainBase, IAvailabilityPeriod
    {
        public string Name { get; set; }
        public DateTime OpenFrom { get; set; }
        public DateTime OpenUntil { get; set; }
    }
}