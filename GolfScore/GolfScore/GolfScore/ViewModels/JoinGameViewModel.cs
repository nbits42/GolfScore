using GalaSoft.MvvmLight.Command;
using GlobalContracts.Enumerations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.Helpers;
using TeeScore.Translations;
using Xamarin.Forms;
using ZXing;

namespace TeeScore.ViewModels
{
    public class JoinGameViewModel : MyViewModelBase
    {
        private readonly IDialogService _dialogService;
        private int _invitationNumber;
        private bool _isWaiting;
        private RelayCommand _joinGameCommand;
        private string _venue;
        private string _gameType;
        private DateTime _gameDate;
        private bool _showGameData;
        private RelayCommand _scanResultCommand;
        private Result _scanResult;
        private bool _isAnalyzing;
        private bool _isScanning;
        private Game _game;
        private bool _startGame;

        public JoinGameViewModel(IDataService dataService, INavigationService navigationService, IDialogService dialogService) : base(dataService, navigationService)
        {
            _dialogService = dialogService;
        }

        /* =========================================== property: InvitationNumber ====================================== */
        /// <summary>
        /// Sets and gets the InvitationNumber property.
        /// </summary>
        public int InvitationNumber
        {
            get => _invitationNumber;
            set
            {
                if (value == _invitationNumber)
                {
                    return;
                }

                _invitationNumber = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: IsWaiting ====================================== */
        /// <summary>
        /// Sets and gets the IsWaiting property.
        /// </summary>
        public bool IsWaiting
        {
            get => _isWaiting;
            set
            {
                if (value == _isWaiting)
                {
                    return;
                }

                _isWaiting = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: Venue ====================================== */
        /// <summary>
        /// Sets and gets the Venue property.
        /// </summary>
        public string Venue
        {
            get => _venue;
            set
            {
                if (value == _venue)
                {
                    return;
                }

                _venue = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: Location ====================================== */
        /// <summary>
        /// Sets and gets the Location property.
        /// </summary>
        public string GameType
        {
            get => _gameType;
            set
            {
                if (value == _gameType)
                {
                    return;
                }

                _gameType = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: GameDate ====================================== */
        /// <summary>
        /// Sets and gets the GameDate property.
        /// </summary>
        public DateTime GameDate
        {
            get => _gameDate;
            set
            {
                if (value == _gameDate)
                {
                    return;
                }

                _gameDate = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: ShowGameData ====================================== */
        /// <summary>
        /// Sets and gets the ShowGameData property.
        /// </summary>
        public bool ShowGameData
        {
            get => _showGameData;
            set
            {
                if (value == _showGameData)
                {
                    return;
                }

                _showGameData = value;
                RaisePropertyChanged();
            }
        }




        /* =========================================== RelayCommand: GetGameCommand ====================================== */
        /// <summary>
        /// Executes the GetGame command.
        /// </summary>
        public RelayCommand JoinGameCommand
        {
            get
            {
                return _joinGameCommand
                       ?? (_joinGameCommand = new RelayCommand(
                           async () => { await JoinGame(); }));
            }
        }


        /* =========================================== RelayCommand: ScanResultCommand ====================================== */
        /// <summary>
        /// Executes the ScanResult command.
        /// </summary>
        public RelayCommand ScanResultCommand =>
            _scanResultCommand
            ?? (_scanResultCommand = new RelayCommand(HandleScanResult));

        private void HandleScanResult()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {

                var text = ScanResult.Text;
                if (string.IsNullOrEmpty(text))
                {
                    _dialogService.ShowError(Messages.ScanReturnedNoData, Labels.title_join_with_qr_code);
                    return;
                }

                if (!text.StartsWith(Properties.Resources.QrCodeUrl))
                {
                    _dialogService.ShowError(Messages.InvalidQrCode, Labels.title_join_with_qr_code);
                    return;
                }

                var number = text.Replace(Properties.Resources.QrCodeUrl, string.Empty);
                var valid = int.TryParse(number, out var invitationNumber);
                if (!valid)
                {
                    _dialogService.ShowError(Messages.InvalidInvitationNumber, Labels.title_join_with_qr_code);
                    return;
                }
                IsScanning = false;
                IsAnalyzing = false;

                InvitationNumber = invitationNumber;
                await JoinGame();
            });

        }

        /* =========================================== property: IsScanning ====================================== */
        /// <summary>
        /// Sets and gets the IsScanning property.
        /// </summary>
        public bool IsScanning
        {
            get => _isScanning;
            set
            {
                if (value == _isScanning)
                {
                    return;
                }

                _isScanning = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: IsAnalyzing ====================================== */
        /// <summary>
        /// Sets and gets the IsAnalyzing property.
        /// </summary>
        public bool IsAnalyzing
        {
            get => _isAnalyzing;
            set
            {
                if (value == _isAnalyzing)
                {
                    return;
                }

                _isAnalyzing = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== property: ScanResult ====================================== */
        /// <summary>
        /// Sets and gets the ScanResult property.
        /// </summary>
        public ZXing.Result ScanResult
        {
            get => _scanResult;
            set
            {
                if (value == _scanResult)
                {
                    return;
                }

                _scanResult = value;
                RaisePropertyChanged();
            }
        }

        private async Task JoinGame()
        {
            IsWaiting = true;
            var game = await DataService.GetGame(InvitationNumber);
            if (game == null)
            {
                _dialogService.ShowError(Messages.GameNotFoundByInvitationNumber, Labels.title_join_game);
                IsWaiting = false;
                return;
            }

            var players = await DataService.GetPlayersForGame(game.Id);
            if (players.Any(x => x.Id == Settings.MyPlayerId))
            {
                _dialogService.ShowError(Messages.PlayerAlreadyJoined, Labels.title_join_game);
            }

            var result = await _dialogService.ShowMessage(string.Format(Messages.GameFoundByInvitationNumber, game.GameTypeName, game.VenueName, game.GameDate, Environment.NewLine, "    "),
                Labels.title_join_game, Labels.btn_yes, Labels.btn_no);

            if (!result)
            {
                IsWaiting = false;
                return;
            }

            Venue = game.VenueName;
            GameType = game.GameTypeName;
            GameDate = game.GameDate;
            ShowGameData = true;
            IsWaiting = true;

            await SaveMeAsGamePlayer(game.Id);
        }

        /* =========================================== property: Game ====================================== */
        /// <summary>
        /// Sets and gets the Game property.
        /// </summary>
        public Game Game
        {
            get => _game;
            set
            {
                if (value == _game)
                {
                    return;
                }

                _game = value;
                RaisePropertyChanged();
            }
        }



        private async Task SaveMeAsGamePlayer(string gameId)
        {
            var gamePlayer = new GamePlayer
            {
                GameId = gameId,
                PlayerId = Settings.MyPlayerId,
                PlayerRole = PlayerRole.Player
            };
            await DataService.SaveGamePlayer(gamePlayer, true);
        }

        private async Task ContinuousPlayerPolling()
        {
            var startTime = DateTime.Now;
            var maxWaitMinutes = 5;
            IsWaiting = true;
            var stopwatch = new Stopwatch();
            var pollCount = 0;
            while (IsWaiting)
            {
                stopwatch.Start();
                await DataService.SyncAsync().ConfigureAwait(true);
                var players = await DataService.GetPlayersForGame(Game.Id).ConfigureAwait(true);
                stopwatch.Stop();
                Debug.Write($"{pollCount++} Poll for players. Time elapsed: {stopwatch.Elapsed.TotalSeconds}");
                if (players.Count == Game.InvitedPlayersCount)
                {
                    IsWaiting = false;
                    StartGame = true;
                }

                if (_isWaiting)
                {
                    if (DateTime.Now.Subtract(startTime).Minutes > maxWaitMinutes)
                    {
                        IsWaiting = false;
                    }
                }
                // Update the UI (because of async/await magic, this is still in the UI thread!)
                if (IsWaiting)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }

        /* =========================================== property: StartGame ====================================== */
        /// <summary>
        /// Sets and gets the StartGame property.
        /// </summary>
        public bool StartGame
        {
            get => _startGame;
            set
            {
                if (value == _startGame)
                {
                    return;
                }

                _startGame = value;
                RaisePropertyChanged();
            }
        }


    }
}
