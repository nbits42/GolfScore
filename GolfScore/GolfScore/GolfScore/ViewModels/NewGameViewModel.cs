using GalaSoft.MvvmLight.Command;
using GlobalContracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;
using TeeScore.Services;
using TeeScore.Validation;
using Xamarin.Forms;

namespace TeeScore.ViewModels
{
    public class NewGameViewModel : ValidatableViewModelBase
    {
        private ObservableCollection<VenueDto> _venues = new ObservableCollection<VenueDto>();
        private NewGameDto _game = new NewGameDto();
        private PlayerDto _myPlayer = null;
        private VenueDto _selectedVenue = null;
        private string _venueSearch;
        private bool _nextPageEnabled;
        private bool _previousPageEnabled;
        private CreateGamePage _currentPage;
        private CreateGamePage _nextPage;
        private RelayCommand _nextPageCommand;
        private RelayCommand _previousPageCommand;
        private int _invitationNumber = 0;
        private int _teeCount = 1;
        private int _startTee = 1;
        private GameType _gameType = GameType.Golf;
        private bool _doCheck = false;
        private List<EnumNameValue<GameType>> _gameTypesList;
        private int _selectedGameTypeIndex;
        private int _currentPageIndex;
        private int _selectedPlayerSelectionIndex;
        private PlayerSelection _playerSelection = PlayerSelection.Manual;
        private List<EnumNameValue<PlayerSelection>> _playerSelectionList;
        private RelayCommand _inviteCommand;
        private bool _invitationRunning = false;
        private ObservableCollection<PlayerDto> _players = new ObservableCollection<PlayerDto>();
        private int _playersCount;
        private RelayCommand _startGameCommand;
        private bool _loaded = false;
        private GamePlayer _myGamePlayer = null;
        private bool _startButtonIsEnabled;
        private string _startText;
        private bool _startButtonIsVisible;
        private bool _startingIsBusy;


        public NewGameViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
            PropertyChanged += NewGameViewModel_PropertyChanged;
            LoadGameTypes();
            LoadPlayerSelections();
        }

        public event EventHandler GameStarted;

        public async Task NewGameAsync()
        {
            await LoadAsync();

            _myGamePlayer = null;
            _doCheck = false;

            Settings.CurrentScoreIx = 0;
            Game = new NewGameDto();
            SelectedVenue = null;
            GameType = Settings.LastGameType;
            TeeCount = Settings.LastTeeCount;
            StartTee = 1;
            InvitationNumber = 0;
            PlayersCount = Settings.LastPlayersCount;
            Players.Clear();

            Players.Add(MyPlayer);

            CurrentPage = CreateGamePage.VenueSelection;
            CurrentPageIndex = (int)CurrentPage;
            StartButtonIsVisible = true;
            StartingIsBusy = false;
            _doCheck = true;
            CheckNextPage();
        }

        public async Task ResumeNewGameAsync(string gameId)
        {
            await LoadAsync();

            _doCheck = false;
            Settings.CurrentScoreIx = 0;
            _myGamePlayer = new GamePlayer {GameId = Game.Game.Id, PlayerId = MyPlayer.Id};
            Game = await GetGame(gameId).ConfigureAwait(true);
            Players = new ObservableCollection<PlayerDto>(Game.Players);
            SelectedGameTypeIndex = _gameTypesList.FindIndex(x => x.Value == Game.Game.GameType);
            SelectedPlayerSelectionIndex = _playerSelectionList.FindIndex(x => x.Value == Game.Game.PlayerSelection);
            TeeCount = Game.Game.TeeCount;
            StartTee = Game.Game.StartTee;
            PlayersCount = Game.Game.InvitedPlayersCount;

            _doCheck = true;
            CurrentPage = CreateGamePage.VenueSelection;
            CurrentPageIndex = (int)CurrentPage;
            SelectedVenue = FindVenue(Game.Venue);
            StartButtonIsVisible = true;
            StartingIsBusy = false;
        }

        private VenueDto FindVenue(VenueDto gameVenue)
        {
            return Venues.FirstOrDefault(x => x.Id == gameVenue.Id);
        }

        private async Task<NewGameDto> GetGame(string gameId)
        {
            return await DataService.GetNewGame(gameId);
        }

        private void NewGameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedVenue):
                    CheckNextPage();
                    break;
                case nameof(StartTee):
                case nameof(TeeCount):
                case nameof(PlayersCount):
                    CheckNextPage();
                    break;
                case nameof(GameType):
                    CheckNextPage();
                    SelectedGameTypeIndex = _gameTypesList.FindIndex(x => x.Value == GameType);
                    break;
                case nameof(SelectedGameTypeIndex):
                    _gameType = _gameTypesList[SelectedGameTypeIndex].Value;
                    CheckNextPage();
                    break;
                case nameof(PlayerSelection):
                    CheckNextPage();
                    SelectedPlayerSelectionIndex = _playerSelectionList.FindIndex(x => x.Value == PlayerSelection);
                    break;
                case nameof(SelectedPlayerSelectionIndex):
                    _playerSelection = _playerSelectionList[SelectedPlayerSelectionIndex].Value;

                    if (_playerSelection == PlayerSelection.ByInvitationNumber)
                    {
                        if (_invitationNumber == 0)
                        {
                            InvitationNumber = GenerateInvitationNumber();
                        }
                    }
                    CheckNextPage();
                    break;
                case nameof(Players):
                    CheckNextPage();
                    break;
                
           }
        }

        private int GenerateInvitationNumber()
        {
            var r = new Random();
            return r.Next(1000, 9999);
        }

        private void CheckNextPage()
        {
            if (!_doCheck)
            {
                return;
            }
            UpdateGame();
            _nextPage = GameStateService.GetNextNewGamePage(Game, _currentPage);
            NextPageEnabled = _nextPage > _currentPage && _currentPage < CreateGamePage.Ready;
            PreviousPageEnabled = _currentPage > CreateGamePage.VenueSelection && _currentPage <= CreateGamePage.Ready;
        }

        private void UpdateGame()
        {
            if (SelectedVenue == null)
            {
                return;
            }
            Game.Venue = SelectedVenue;
            Game.Game.GameType = GameType;
            Game.Game.InvitationNumber = InvitationNumber;
            Game.Game.InvitedPlayersCount = PlayersCount;
            Game.Game.StartTee = StartTee;
            Game.Game.TeeCount = TeeCount;
            Game.Game.VenueId = SelectedVenue?.Id;
            Game.Game.PlayerSelection = PlayerSelection;
            Game.Players = new List<PlayerDto>(_players);
            Game.Game.VenueName = SelectedVenue?.Name;
            Game.Game.PlayerNames = string.Join(" \u2022 ", _players.Select(x => x.Name));
        }

        public async Task LoadAsync()
        {
            if (_loaded)
            {
                return;
            }
            try
            {
                await LoadPlayerAsync().ConfigureAwait(true);
                if (Venues == null || !Venues.Any())
                {
                    await LoadVenuesAsync().ConfigureAwait(true);
                }

                _loaded = true;
            }
            catch (Exception e)
            {
                ErrorReportingService.ReportError(this, e);
            }

        }

        private void LoadGameTypes()
        {

            if (_gameTypesList == null)
            {
                _gameTypesList = EnumHelper<GameType>.GetNames();
                GameTypes.AddRange(_gameTypesList.Select(x => x.Name));
                RaisePropertyChanged(() => GameTypes);
            }
        }
        private void LoadPlayerSelections()
        {

            if (_playerSelectionList == null)
            {
                _playerSelectionList = EnumHelper<PlayerSelection>.GetNames();
                PlayerSelections.AddRange(_playerSelectionList.Select(x => x.Name));
                RaisePropertyChanged(() => PlayerSelections);
            }
        }

        public async Task LoadVenuesAsync()
        {
            var allVenues = await DataService.GetVenues().ConfigureAwait(true);
            Venues = new ObservableCollection<VenueDto>(allVenues.OrderBy(x => x.Name).ToList());
        }

        private async Task LoadPlayerAsync()
        {
            if (MyPlayer == null)
            {
                MyPlayer = await DataService.GetPlayer(Settings.MyPlayerId).ConfigureAwait(true);
            }
            
        }

        /* =========================================== property: Venues ====================================== */
        /// <summary>
        /// Sets and gets the Venues property.
        /// </summary>
        public ObservableCollection<VenueDto> Venues
        {
            get => _venues;
            set
            {
                if (value == _venues)
                {
                    return;
                }

                _venues = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: Game ====================================== */
        /// <summary>
        /// Sets and gets the Game property.
        /// </summary>
        public NewGameDto Game
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

        /* =========================================== property: MyPlayer ====================================== */
        /// <summary>
        /// Sets and gets the MyPlayer property.
        /// </summary>
        public PlayerDto MyPlayer
        {
            get => _myPlayer;
            set
            {
                if (value == _myPlayer)
                {
                    return;
                }

                _myPlayer = value;
                RaisePropertyChanged();
            }
        }


        /* =========================================== property: SelectedVenue ====================================== */
        /// <summary>
        /// Sets and gets the SelectedVenue property.
        /// </summary>
        public VenueDto SelectedVenue
        {
            get => _selectedVenue;
            set
            {
                if (value == _selectedVenue)
                {
                    return;
                }

                _selectedVenue = value;
                RaisePropertyChanged();
            }
        }


        /* =========================================== property: VenueSearch ====================================== */
        /// <summary>
        /// Sets and gets the VenueSearch property.
        /// </summary>
        public string VenueSearch
        {
            get => _venueSearch;
            set
            {
                if (value == _venueSearch)
                {
                    return;
                }

                _venueSearch = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: NextPageEnabled ====================================== */
        /// <summary>
        /// Sets and gets the NextPageEnabled property.
        /// </summary>
        public bool NextPageEnabled
        {
            get => _nextPageEnabled;
            set
            {
                if (value == _nextPageEnabled)
                {
                    return;
                }

                _nextPageEnabled = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: PreviousPageEnabled ====================================== */
        /// <summary>
        /// Sets and gets the PreviousPageEnabled property.
        /// </summary>
        public bool PreviousPageEnabled
        {
            get => _previousPageEnabled;
            set
            {
                if (value == _previousPageEnabled)
                {
                    return;
                }

                _previousPageEnabled = value;
                RaisePropertyChanged();
            }
        }


        /* =========================================== property: CurrentPage ====================================== */
        /// <summary>
        /// Sets and gets the CurrentPage property.
        /// </summary>
        public CreateGamePage CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value == _currentPage)
                {
                    return;
                }

                _currentPage = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: CurrentPageIndex ====================================== */
        /// <summary>
        /// Sets and gets the CurrentPageIndex property.
        /// </summary>
        public int CurrentPageIndex
        {
            get => _currentPageIndex;
            set
            {
                if (value == _currentPageIndex)
                {
                    return;
                }

                _currentPageIndex = value;
                RaisePropertyChanged();
            }
        }
        

        /* =========================================== RelayCommand: NextPageCommand ====================================== */
        /// <summary>
        /// Executes the NextPage command.
        /// </summary>
        public RelayCommand NextPageCommand => _nextPageCommand ?? (_nextPageCommand = new RelayCommand(async () => await NextPage()));

        private async Task NextPage()
        {
            if (NextPageEnabled)
            {
                CurrentPage = CurrentPage + 1;
                CurrentPageIndex = (int)CurrentPage;
                CheckNextPage();
                await SaveGame();
            }
        }


        /* =========================================== RelayCommand: PreviousPageCommand ====================================== */
        /// <summary>
        /// Executes the PreviousPage command.
        /// </summary>
        public RelayCommand PreviousPageCommand => _previousPageCommand ?? (_previousPageCommand = new RelayCommand(PreviousPage));

        private void PreviousPage()
        {
            if (PreviousPageEnabled)
            {
                CurrentPage = CurrentPage - 1;
                CurrentPageIndex = (int)CurrentPage;
                CheckNextPage();
            }
        }


        protected override void AddValidations()
        {

        }

        public override bool Validate()
        {
            var validateTeeCount = ValidateStartTeeCommand();
            return validateTeeCount && SelectedVenue != null;
        }

        private bool ValidateStartTeeCommand()
        {
            return StartTee <= TeeCount;
        }


        /* =========================================== property: PlayersCount ====================================== */
        /// <summary>
        /// Sets and gets the PlayersCount property.
        /// </summary>
        public int PlayersCount
        {
            get => _playersCount;
            set
            {
                if (value == _playersCount)
                {
                    return;
                }

                _playersCount = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(NewPlayersCanBeAdded));
            }
        }


        /* =========================================== property: TeeCount ====================================== */
        /// <summary>
        /// Sets and gets the TeeCount property.
        /// </summary>
        public int TeeCount
        {
            get => _teeCount;
            set
            {
                if (value == _teeCount)
                {
                    return;
                }

                _teeCount = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: StartTee ====================================== */
        /// <summary>
        /// Sets and gets the StartTee property.
        /// </summary>
        public int StartTee
        {
            get => _startTee;
            set
            {
                if (value == _startTee)
                {
                    return;
                }

                _startTee = value;
                RaisePropertyChanged();
            }
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




        /* =========================================== property: GameType ====================================== */
        /// <summary>
        /// Sets and gets the GameType property.
        /// </summary>
        public GameType GameType
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

        /* =========================================== property: SelectedGameTypeIndex ====================================== */
        /// <summary>
        /// Sets and gets the SelectedGameTypeIndex property.
        /// </summary>
        public int SelectedGameTypeIndex
        {
            get => _selectedGameTypeIndex;
            set
            {
                if (value == _selectedGameTypeIndex)
                {
                    return;
                }

                _selectedGameTypeIndex = value;
                RaisePropertyChanged();
            }
        }

        public List<string> PlayerSelections { get; set; } = new List<string>();

        /* =========================================== property: PlayerSelection ====================================== */
        /// <summary>
        /// Sets and gets the PlayerSelection property.
        /// </summary>
        public PlayerSelection PlayerSelection
        {
            get => _playerSelection;
            set
            {
                if (value == _playerSelection)
                {
                    return;
                }

                _playerSelection = value;
                RaisePropertyChanged();

            }
        }

        /* =========================================== property: SelectedPlayerSelectionIndex ====================================== */
        /// <summary>
        /// Sets and gets the SelectedPlayerSelectionIndex property.
        /// </summary>
        public int SelectedPlayerSelectionIndex
        {
            get => _selectedPlayerSelectionIndex;
            set
            {
                if (value == _selectedPlayerSelectionIndex)
                {
                    return;
                }

                _selectedPlayerSelectionIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => HasAutomaticPlayerSelection);
                RaisePropertyChanged(() => HasManualPlayerSelection);
            }
        }

        public List<string> GameTypes { get; set; } = new List<string>();

        /* =========================================== property: Players ====================================== */
        /// <summary>
        /// Sets and gets the Players property.
        /// </summary>
        public ObservableCollection<PlayerDto> Players
        {
            get => _players;
            set
            {
                if (value == _players)
                {
                    return;
                }

                _players = value;
                RaisePropertyChanged();
            }
        }



        public bool HasAutomaticPlayerSelection => _playerSelection != PlayerSelection.Manual;
        public bool HasManualPlayerSelection => _playerSelection == PlayerSelection.Manual;


        /* =========================================== RelayCommand: InviteCommand ====================================== */
        /// <summary>
        /// Executes the Invite command.
        /// </summary>
        public RelayCommand InviteCommand
        {
            get
            {
                return _inviteCommand
                       ?? (_inviteCommand = new RelayCommand(
                           async () => { await Invite(); }));
            }
        }

        private async Task Invite()
        {
            await SaveGame();
            if (_invitationRunning)
            {
                InvitationIsRunning = false;
                return;
            }

            await ContinuousPlayerPolling().ConfigureAwait(true);
        }

        private async Task SaveGame()
        {
            UpdateGame();
            Game.Game = await DataService.SaveGame(Game.Game, true).ConfigureAwait(false);
            if (_myGamePlayer == null)
            {
                _myGamePlayer = new GamePlayer
                {
                    GameId = Game.Game.Id,
                    PlayerId = MyPlayer.Id
                };
                _myGamePlayer = await DataService.SaveGamePlayer(_myGamePlayer).ConfigureAwait(false);
            }
            Settings.CurrentGameId = Game.Game.Id;
        }

        private async Task ContinuousPlayerPolling()
        {
            var startTime = DateTime.Now;
            var maxWaitMinutes = 10;
            InvitationIsRunning = true;
            while (InvitationIsRunning)
            {
                var players = await DataService.GetPlayersForGame(Game.Game.Id).ConfigureAwait(true);
                if (players.Count == PlayersCount)
                {
                    InvitationIsRunning = false;
                }

                if (_invitationRunning)
                {
                    if (DateTime.Now.Subtract(startTime).Minutes > maxWaitMinutes)
                    {
                        InvitationIsRunning = false;
                    }
                }
                // Update the UI (because of async/await magic, this is still in the UI thread!)
                if (InvitationIsRunning)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }

        /* =========================================== property: InvitationIsRunning ====================================== */
        /// <summary>
        /// Sets and gets the InvitationIsRunning property.
        /// </summary>
        public bool InvitationIsRunning
        {
            get => _invitationRunning;
            set
            {
                if (value == _invitationRunning)
                {
                    return;
                }

                _invitationRunning = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: NewPlayersCanBeAdded ====================================== */
        /// <summary>
        /// Sets and gets the NewPlayersCanBeAdded property.
        /// </summary>
        public bool NewPlayersCanBeAdded => _players.Count < PlayersCount;


        /* =========================================== RelayCommand: StartGameCommand ====================================== */
        /// <summary>
        /// Executes the StartGame command.
        /// </summary>
        public RelayCommand StartGameCommand => _startGameCommand
                                                ?? (_startGameCommand = new RelayCommand(async () => await StartGame()));


        /* =========================================== property: StartButtonIsVisible ====================================== */
        /// <summary>
        /// Sets and gets the StartButtonIsVisible property.
        /// </summary>
        public bool StartButtonIsVisible
        {
            get => _startButtonIsVisible;
            set
            {
                if (value == _startButtonIsVisible)
                {
                    return;
                }

                _startButtonIsVisible = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: StartingIsBusy ====================================== */
        /// <summary>
        /// Sets and gets the StartingIsBusy property.
        /// </summary>
        public bool StartingIsBusy
        {
            get => _startingIsBusy;
            set
            {
                if (value == _startingIsBusy)
                {
                    return;
                }

                _startingIsBusy = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== property: StartText ====================================== */
        /// <summary>
        /// Sets and gets the StartText property.
        /// </summary>
        public string StartText
        {
            get => _startText;
            set
            {
                if (value == _startText)
                {
                    return;
                }

                _startText = value;
                RaisePropertyChanged();
            }
        }



        public async Task StartGame()
        {
            StartButtonIsVisible = false;
            StartingIsBusy = true;
            PreviousPageEnabled = false;
            StartText = Translations.Labels.lbl_starting_getting_data;
            UpdateGame();

            Settings.LastGameType = Game.Game.GameType;
            Settings.LastPlayersCount = Game.Game.InvitedPlayersCount;
            Settings.LastTeeCount = Game.Game.TeeCount;
            Settings.CurrentGameId = Game.Game.Id;

            Game.Game.GameStatus = GameStatus.Started;
            Game.Game.CurrentTee = (Game.Game.StartTee - 1 ) * Game.Game.InvitedPlayersCount;
            await SaveGame();

            for (var i = 1; i <= Game.Game.TeeCount; i++)
            {
                var tee = new TeeDto
                {
                    GameId = Game.Game.Id,
                    Number = i.ToString("D2")
                };
                tee = await DataService.SaveTee(tee);
                foreach (var player in Game.Players.OrderBy(x=>x.Abbreviation))
                {
                    var teeScore = new ScoreDto
                    {
                        GameId = Game.Game.Id,
                        TeeId = tee.Id,
                        PlayerId = player.Id,
                        PlayerAbbreviation = player.Abbreviation,
                        Putts = 0,
                    };
                    await DataService.SaveScore(teeScore);
                }
            }

            StartText = Translations.Labels.lbl_starting_saving_data;

            await DataService.SyncAsync();
            //OnGameStarted();
            StartText = Translations.Labels.lbl_starting_starting;
        }



        protected virtual void OnGameStarted()
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteGamePlayerAsync(PlayerDto player)
        {
            if (player != null)
            {
                await DataService.DeleteGamePlayer(Game.Game.Id, player.Id);
                Players.Remove(player);
            }
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(Players));
            RaisePropertyChanged(nameof(Venues));
        }
    }
}
