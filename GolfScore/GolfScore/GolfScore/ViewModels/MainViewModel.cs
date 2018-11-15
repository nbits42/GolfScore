using GolfScore.Contracts;
using GolfScore.Domain;
using GolfScore.DTO;
using GolfScore.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GolfScore.ViewModels
{
    public class MainViewModel : MyViewModelBase
    {
        private Player _player;
        private bool _isInitialized;

        public MainViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
            MyPlayer = new Player();
        }

        public async Task SavePlayer()
        {
            if (MyPlayer.IsNew)
            {
                var player = await DataService.NewPlayer(MyPlayer);
                Settings.MyPlayerId = player.Id;
                MyPlayer = player;
            }
            else
            {
                await DataService.SavePlayer(MyPlayer);
            }

        }

        internal void ClearMyPlayer()
        {
            MyPlayer.Clear();
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

        public async Task Load()
        {
            if (!_isInitialized)
            {
                await DataService.InitializeAsync();
                _isInitialized = true;
            }
            await LoadMyPlayerAsync();
            await LoadGames();
        }

        private async Task LoadMyPlayerAsync()
        {
            if (string.IsNullOrEmpty(Settings.MyPlayerId))
            {
                return;
            }
            var player = await DataService.GetPlayer(Settings.MyPlayerId);
            if (player != null)
            {
                MyPlayer = player;
            }
        }

        private async Task LoadGames()
        {
            var playerId = Settings.MyPlayerId;

            var games = await DataService.GetGames(playerId);
            Games.Clear();
            foreach (var game in games)
            {
                Games.Add(new GameDto
                {
                    Game = game,
                    Venue = await DataService.GetVenue(game.VenueId),
                    Players = await DataService.GetPlayersForGame(game.Id),
                });
            }

        }
    }
}
