using GalaSoft.MvvmLight.Command;
using Syncfusion.DataSource.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;
using TeeScore.Services;

namespace TeeScore.ViewModels
{
    public class NewGameViewModel : MyViewModelBase
    {
        private List<Venue> _allVenues;
        private ObservableCollection<Venue> _venues;
        private GameDto _game = new GameDto();
        private Player _myPlayer;
        private Venue _seletedVenue;
        private string _venueSearch;
        private bool _nextPageEnabled;
        private bool _previousPageEnabled;
        private CreateGamePage _currentPage;
        private CreateGamePage _nextPage;
        private RelayCommand _nextPageCommand;
        private RelayCommand _previousPageCommand;

        public NewGameViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
            PropertyChanged += NewGameViewModel_PropertyChanged;

            _game.Game.PropertyChanged += Game_PropertyChanged;
        }

        private void Game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_game.Game.ConnectedPlayersCount):
                case nameof(_game.Game.InvitedPlayersCount):
                case nameof(_game.Game.TeeCount):
                case nameof(_game.Game.GameType):
                case nameof(_game.Game.InvitationNumber):
                    CheckNextPage();
                    break;
            }
        }

        private void NewGameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VenueSearch):
                    FilterVenues();
                    break;
                case nameof(SeletedVenue):
                    CheckNextPage();
                    break;
            }
        }

        private void CheckNextPage()
        {
            _nextPage = GameStateService.GetNextNewGamePage(Game, _currentPage);
            NextPageEnabled = _nextPage > _currentPage;
            PreviousPageEnabled = _currentPage > CreateGamePage.VenueSelection && _currentPage < CreateGamePage.Ready;
        }

        private void FilterVenues()
        {
            Venues = string.IsNullOrEmpty(_venueSearch)
                ? new ObservableCollection<Venue>(_allVenues.OrderBy(x => x.Name))
                : new ObservableCollection<Venue>(_allVenues.Where(x => x.Name.Contains(VenueSearch)).OrderBy(x => x.Name));
        }

        public async Task LoadAsync()
        {
            await LoadPlayer();
            await LoadVenues();
        }

        private async Task LoadVenues()
        {
            _allVenues = await DataService.GetVenues();
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


        /* =========================================== property: SeletedVenue ====================================== */
        /// <summary>
        /// Sets and gets the SeletedVenue property.
        /// </summary>
        public Venue SeletedVenue
        {
            get => _seletedVenue;
            set
            {
                if (value == _seletedVenue)
                {
                    return;
                }

                _seletedVenue = value;
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
                CurrentPage = _currentPage--;
            }
        }
    }
}
