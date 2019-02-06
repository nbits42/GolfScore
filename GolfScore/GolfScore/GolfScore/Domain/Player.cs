using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class Player : DomainBase, IPlayer
    {
        public string Abbreviation { get; set; }
        public string AvatarUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }

        internal void Clear()
        {
            Name = string.Empty;
            ImageUrl = string.Empty;
            AvatarUrl = string.Empty;
            Abbreviation = string.Empty;
        }
    }
}
