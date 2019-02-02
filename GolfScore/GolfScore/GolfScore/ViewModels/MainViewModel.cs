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
        private PlayerDto _player = new PlayerDto();
        private bool _isInitialized;
        private ObservableCollection<Game> _games;
        private bool _isBusy;

        public MainViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
            IsBusy = true;
        }

        /* =========================================== property: Player ====================================== */
        /// <summary>
        /// Sets and gets the Player property.
        /// </summary>
        public PlayerDto MyPlayer
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

        /* =========================================== property: IsBusy ====================================== */
        /// <summary>
        /// Sets and gets the IsBusy property.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (value == _isBusy)
                {
                    return;
                }

                _isBusy = value;
                RaisePropertyChanged();
            }
        }


/* =========================================== property: Games ====================================== */
        /// <summary>
        /// Sets and gets the Games property.
        /// </summary>
        public ObservableCollection<Game> Games
        {
            get => _games;
            set
            {
                if (value == _games)
                {
                    return;
                }

                _games = value;
                RaisePropertyChanged();
            }
        }

        public async Task HideGameAsync(Game item)
        {
            if (item == null)
            {
                return;
            }

            Games.Remove(item);
            await HideGamePlayerAsync(item, MyPlayer);
        }

        private async Task HideGamePlayerAsync(Game item, PlayerDto player)
        {
            var gamePlayer = await DataService.GetGamePlayer(item.Id, player.Id);
            if (gamePlayer != null)
            {
                gamePlayer.Hide = true;
                await DataService.SaveGamePlayer(gamePlayer);
            }
        }

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

                await InitializeTimer();
            }
            catch (Exception e)
            {
                ErrorReportingService.ReportError(this, e);
            }

            IsBusy = false;
        }

        private async Task InitializeTimer()
        {
            await WaitAndExecute(2000, async () => { await LoadGamesAsync(); });
        }

        private async Task WaitAndExecute(int milisecs, Action action)
        {
            await Task.Delay(milisecs);
            action();
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

            Games = new ObservableCollection<Game>(await DataService.GetGames(playerId).ConfigureAwait(true));
        }

        public async Task<Game> GetGame(string gameId)
        {
            return await DataService.GetGame(gameId);
        }
    }
}
