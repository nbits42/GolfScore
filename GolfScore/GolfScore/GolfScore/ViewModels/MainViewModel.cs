using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;

namespace TeeScore.ViewModels
{
    public class MainViewModel : MyViewModelBase
    {
        private Player _player = new Player();
        private bool _isInitialized;

        public MainViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
          
        }

        /* =========================================== property: Player ====================================== */
        /// <summary>
        /// Sets and gets the Player property.
        /// </summary>
        public Player MyPlayer
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

        public ObservableCollection<GameDto> Games { get; set; } = new ObservableCollection<GameDto>();

        public async Task LoadAsync()
        {
            if (!_isInitialized)
            {
                await DataService.InitializeAsync().ConfigureAwait(false);
                _isInitialized = true;
            }
            await LoadMyPlayerAsync().ConfigureAwait(false);
            await LoadGamesAsync().ConfigureAwait(false);
        }

        private async Task LoadMyPlayerAsync()
        {
            if (string.IsNullOrEmpty(Settings.MyPlayerId))
            {
                return;
            }
            MyPlayer = await DataService.GetPlayer(Settings.MyPlayerId);
        }

        private async Task LoadGamesAsync()
        {
            var playerId = Settings.MyPlayerId;

            var games = await DataService.GetGames(playerId).ConfigureAwait(false);
            Games.Clear();
            foreach (var game in games)
            {
                Games.Add(new GameDto
                {
                    Game = game,
                    Venue = await DataService.GetVenue(game.VenueId).ConfigureAwait(false),
                    Players = await DataService.GetPlayersForGame(game.Id).ConfigureAwait(false),
                });
            }

        }
    }
}
