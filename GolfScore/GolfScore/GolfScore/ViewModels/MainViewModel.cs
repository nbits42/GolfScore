using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;
using TeeScore.Services;

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
            try
            {
                if (!_isInitialized)
                {
                    await DataService.InitializeAsync().ConfigureAwait(true);
                    _isInitialized = true;
                }
                await LoadMyPlayerAsync().ConfigureAwait(true);
                await LoadGamesAsync().ConfigureAwait(true);
            }
            catch (Exception e)
            {
                ErrorReportingService.ReportError(this, e);
            }
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

            var games = await DataService.GetGames(playerId).ConfigureAwait(true);
            Games.Clear();
            foreach (var game in games)
            {
                var venue = await DataService.GetVenue(game.VenueId).ConfigureAwait(true);
                var players = await DataService.GetPlayersForGame(game.Id).ConfigureAwait(true);
                Games.Add(new GameDto
                {
                    Game = game,
                    Venue = venue,
                    Players = players,
                });
            }

        }
    }
}
