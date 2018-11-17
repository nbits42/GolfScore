using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class GamePlayer : DomainBase, IGamePlayer
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
    }
}