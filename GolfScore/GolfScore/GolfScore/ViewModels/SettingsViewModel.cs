using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.Helpers;

namespace TeeScore.ViewModels
{
    public class SettingsViewModel : MyViewModelBase
    {
        private Player _player = new Player();

        public SettingsViewModel(IDataService dataservice, INavigationService navigationService) : base(dataservice, navigationService)
        {
        }

        /* =========================================== property: Player ====================================== */
        /// <summary>
        /// Sets and gets the Player property.
        /// </summary>
        public Player Player
        {
            get => _player;
            set
            {
                if (value == _player)
                {
                    return;
                }

                _player = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            var playerId = Settings.MyPlayerId;
            if (string.IsNullOrEmpty(playerId))
            {
                Player = new Player();
            }
            else
            {
                Player = await DataService.GetPlayer(playerId);
                if (Player == null)
                {
                    Player = new Player();
                }
            }
        }

        public async Task SaveAsync()
        {
            Player = await DataService.SavePlayer(Player);
            Settings.MyPlayerId = Player.Id;
        }
    }
}
