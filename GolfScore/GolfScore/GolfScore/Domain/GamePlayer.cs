using GlobalContracts.Enumerations;
using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class GamePlayer : DomainBase, IGamePlayer
    {
        public GamePlayer()
        {
            PlayerRole = PlayerRole.Owner;
            Hide = false;
        }

        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public PlayerRole PlayerRole { get; set; }
        public bool Hide { get; set; }
    }
}