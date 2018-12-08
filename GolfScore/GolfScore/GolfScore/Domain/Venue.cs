using GlobalContracts.Interfaces;
using System.ComponentModel;
using AutoMapper.Mappers;

namespace TeeScore.Domain
{
    public class Venue : DomainBase, IVenue
    {
        public string ImageUrl { get; set; }
        public double Lat { get; set; } = 0;
        public double Long { get; set; } = 0;
        public string ThumbnailUrl { get; set; }
        public string OwnerId { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Location: {Location}, Lat: {Lat}, Long: {Long}";
        }
    }
}