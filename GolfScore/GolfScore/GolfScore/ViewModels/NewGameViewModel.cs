﻿using GalaSoft.MvvmLight.Command;
using GlobalContracts.Enumerations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;
using TeeScore.Services;
using TeeScore.Validation;

namespace TeeScore.ViewModels
{
    public class NewGameViewModel : ValidatableViewModelBase
    {
        private List<Venue> _allVenues = new List<Venue>();
        private ObservableCollection<Venue> _venues = new ObservableCollection<Venue>();
        private GameDto _game = new GameDto();
        private Player _myPlayer = new Player();
        private Venue _selectedVenue = null;
        private string _venueSearch;
        private bool _nextPageEnabled;
        private bool _previousPageEnabled;
        private CreateGamePage _currentPage;
        private CreateGamePage _nextPage;
        private RelayCommand _nextPageCommand;
        private RelayCommand _previousPageCommand;
        private ValidatableObject<int> _invitationNumber = new ValidatableObject<int>();
        private ValidatableObject<int> _invitedPlayersCount = new ValidatableObject<int>();
        private ValidatableObject<int> _teeCount = new ValidatableObject<int>();
        private ValidatableObject<int> _startTee = new ValidatableObject<int>();
        private GameType _gameType = GameType.Golf;
        private bool _doCheck = false;
        private List<EnumNameValue<GameType>> _gameTypesList;
        private int _selectedGameTypeIndex;
        private int _currentPageIndex;

        public NewGameViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
            PropertyChanged += NewGameViewModel_PropertyChanged;
            LoadGameTypes();
        }

        public void NewGame()
        {
            CurrentPage = CreateGamePage.VenueSelection;
            CurrentPageIndex = (int) CurrentPage;
            Game = new GameDto();
            SelectedVenue = null;
            GameType = Settings.LastGameType;
            TeeCount.Value = Settings.LastTeeCount;
            StartTee.Value = 1;
            InvitedPlayersCount.Value = Settings.LastPlayersCount;
        }

        private void NewGameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VenueSearch):
                    FilterVenues();
                    break;
                case nameof(SelectedVenue):
                case nameof(StartTee):
                case nameof(TeeCount):
                case nameof(InvitationNumber):
                case nameof(InvitedPlayersCount):
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
            }
        }

        private void CheckNextPage()
        {
            if (!_doCheck)
            {
                return;
            }
            UpdateGame();
            _nextPage = GameStateService.GetNextNewGamePage(Game, _currentPage);
            NextPageEnabled = _nextPage > _currentPage;
            PreviousPageEnabled = _currentPage > CreateGamePage.VenueSelection && _currentPage < CreateGamePage.Ready;
        }

        private void UpdateGame()
        {
            Game.Venue = SelectedVenue;
            Game.Game.GameType = GameType;
            Game.Game.InvitationNumber = InvitationNumber.Value;
            Game.Game.InvitedPlayersCount = InvitedPlayersCount.Value;
            Game.Game.StartTee = StartTee.Value;
            Game.Game.TeeCount = TeeCount.Value;
        }

        private void FilterVenues()
        {
            Venues = new ObservableCollection<Venue>(_allVenues.OrderBy(x => x.Name));
        }

        public async Task LoadAsync()
        {
            await LoadPlayer();
            await LoadVenues();
            _doCheck = true;
            CheckNextPage();

        }

        private void LoadGameTypes()
        {
            
            if (_gameTypesList == null)
            {
                _gameTypesList = EnumHelper<GameType>.GetNames();
                GameTypes.AddRange(_gameTypesList.Select(x=>x.Name));
                RaisePropertyChanged(() => GameTypes);
            }
        }

        private async Task LoadVenues()
        {
            _allVenues = await DataService.GetVenues();
            FilterVenues();
        }

        private async Task LoadPlayer()
        {
            MyPlayer = await DataService.GetPlayer(Settings.MyPlayerId).ConfigureAwait(false);
        }

        /* =========================================== property: Venues ====================================== */
        /// <summary>
        /// Sets and gets the Venues property.
        /// </summary>
        public ObservableCollection<Venue> Venues
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
        public GameDto Game
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
        public Player MyPlayer
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
        public Venue SelectedVenue
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
                RaisePropertyChanged(() => CurrentPageIndex);
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
        public RelayCommand NextPageCommand => _nextPageCommand ?? (_nextPageCommand = new RelayCommand(NextPage));

        private void NextPage()
        {
            if (NextPageEnabled)
            {
                CurrentPage = _nextPage;
                CurrentPageIndex = (int)CurrentPage;
                CheckNextPage();
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
                CurrentPage = (CreateGamePage) (int)CurrentPage - 1;
                CurrentPageIndex = (int) CurrentPage;
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
            return StartTee.Value <= TeeCount.Value;
        }


        /* =========================================== validatable property: InvitedPlayersCount ====================================== */

        public ValidatableObject<int> InvitedPlayersCount
        {
            get => _invitedPlayersCount;
            set
            {
                _invitedPlayersCount = value;
                RaisePropertyChanged(() => InvitedPlayersCount);
            }
        }

        /* =========================================== validatable property: TeeCount ====================================== */

        public ValidatableObject<int> TeeCount
        {
            get => _teeCount;
            set
            {
                _teeCount = value;
                RaisePropertyChanged(() => TeeCount);
            }
        }

        /* =========================================== validatable property: StartTee ====================================== */

        public ValidatableObject<int> StartTee
        {
            get => _startTee;
            set
            {
                _startTee = value;
                RaisePropertyChanged(() => StartTee);
            }
        }

        /* =========================================== validatable property: InvitationNumber ====================================== */

        public ValidatableObject<int> InvitationNumber
        {
            get => _invitationNumber;
            set
            {
                _invitationNumber = value;
                RaisePropertyChanged(() => InvitationNumber);
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



        public List<string> GameTypes { get; set; } = new List<string>();

    }
}
