using System.Collections.Generic;
using System.Text;
using GlobalContracts.Interfaces;

namespace GolfScore.Domain
{
    public class Player : DomainBase, IPlayer
    {
        public string Abbreviation { get; set; }
        public string AvatarUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }
}
