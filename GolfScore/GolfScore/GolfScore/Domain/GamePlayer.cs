using GlobalContracts.Interfaces;

namespace GolfScore.Domain
{
    public class GamePlayer : DomainBase, IGamePlayer
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
    }
}