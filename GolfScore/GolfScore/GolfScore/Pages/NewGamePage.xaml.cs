using Syncfusion.ListView.XForms;
using System;
using System.Linq;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SwipeDirection = Syncfusion.ListView.XForms.SwipeDirection;

namespace TeeScore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewGamePage : ContentPage
    {
        private readonly NewGameViewModel _vm;
        private string _searchVenueText;
        private string _searchPlayerText;
        private string _gameId = null;
        private PageState _pageState = PageState.New;
        private int _selCount = 0;

        public NewGamePage(string gameId = null)
        {
            InitializeComponent();
            _vm = App.IOC.NewGame;
            BindingContext = _vm;
            _vm.GameStarted += _vm_GameStarted;
            _gameId = gameId;
            
        }

        private async void _vm_GameStarted(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 0)
            {
                await Navigation.PopToRootAsync(true).ConfigureAwait(true);
            }
            else
            {
                //
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            switch (_pageState)
            {
                case PageState.New:
                    _pageState = PageState.Reappearing;

                    if (_gameId == null)
                    {
                        await _vm.NewGameAsync();
                    }
                    else
                    {
                        await _vm.ResumeNewGameAsync(_gameId);
                        _gameId = null;
                    }

                    break;
                case PageState.Reappearing:
                    _vm.Refresh();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            TabView.SelectedIndex = 0;
            return base.OnBackButtonPressed();
        }

        private async void AddVenueButton_OnClicked(object sender, EventArgs e)
        {
            var venuePage = new VenuePage(string.Empty, _vm.Venues);
            await Navigation.PushModalAsync(venuePage);
        }

        private void VenueSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry searchBox && VenuesView.DataSource != null)
            {
                _searchVenueText = searchBox.Text;
                VenuesView.DataSource.Filter = FilterVenues;
                VenuesView.DataSource.RefreshFilter();
            }
        }

        private bool FilterVenues(object obj)
        {
            if (string.IsNullOrEmpty(_searchVenueText))
            {
                return true;
            }

            return obj is Venue venue && (venue.Name.ToLower().Contains(_searchVenueText.ToLower()));
        }

        private bool FilterPlayers(object obj)
        {
            if (string.IsNullOrEmpty(_searchPlayerText))
            {
                return true;
            }

            return obj is Player player && (player.Name.ToLower().Contains(_searchPlayerText.ToLower()));
        }

       
        private async void AddPlayerButton_OnClicked(object sender, EventArgs e)
        {
            var playerPage = new PlayerPage(_vm.Game.Game.Id, _vm.Players);
            await Navigation.PushModalAsync(playerPage);
        }

        private void PlayerSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry searchPlayer && PlayersView.DataSource != null)
            {
                _searchPlayerText = searchPlayer.Text;
                PlayersView.DataSource.Filter = FilterPlayers;
                PlayersView.DataSource.RefreshFilter();
            }
        }

        private void SfListView_OnSwipeStarted(object sender, SwipeStartedEventArgs e)
        {
            e.Cancel = false;
        }

        private void SfListView_OnSwiping(object sender, SwipingEventArgs e)
        {
            if (e.SwipeOffSet > 200)
            {
                e.Handled = true;
            }
        }

        private async void SfListView_OnSwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            if (e.SwipeDirection == SwipeDirection.Right && e.SwipeOffset > 200)
            {
                var item = e.ItemData as PlayerDto;
                await _vm.DeleteGamePlayerAsync(item);
                PlayersView.ResetSwipe(true);
            }
        }

    }
}