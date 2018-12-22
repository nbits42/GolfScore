using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;

namespace TeeScore.ViewModels
{
    public class SettingsViewModel : MyViewModelBase
    {
        private PlayerDto _player = new PlayerDto();

        public SettingsViewModel(IDataService dataservice, INavigationService navigationService) : base(dataservice, navigationService)
        {
        }

        /* =========================================== property: Player ====================================== */
        /// <summary>
        /// Sets and gets the Player property.
        /// </summary>
        public PlayerDto Player
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
                Player = new PlayerDto();
            }
            else
            {
                Player = await DataService.GetPlayer(playerId) ?? 
                         new PlayerDto();
            }
        }

        public async Task SaveAsync()
        {
            Player = await DataService.SavePlayer(Player);
            Settings.MyPlayerId = Player.Id;
        }
    }
}
