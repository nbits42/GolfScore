using Syncfusion.DataSource.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.Helpers;

namespace TeeScore.ViewModels
{
    public class NewGameViewModel : MyViewModelBase
    {
        private List<Venue> _allVenues;
        private ObservableCollection<Venue> _venues;
        private Game _game = new Game();
        private Player _myPlayer;
        private Venue _seletedVenue;
        private string _venueSearch;

        public NewGameViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
            PropertyChanged += NewGameViewModel_PropertyChanged;
        }

        private void NewGameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VenueSearch):
                    FilterVenues();
                    break;
            }
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




    }
}
