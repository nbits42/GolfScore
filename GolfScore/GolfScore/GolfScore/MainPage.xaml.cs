using System;
using GlobalContracts.Enumerations;
using Syncfusion.ListView.XForms;
using TeeScore.Domain;
using TeeScore.Pages;
using TeeScore.ViewModels;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;
using SwipeDirection = Syncfusion.ListView.XForms.SwipeDirection;

namespace TeeScore
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage()
        {
            _viewModel = App.IOC.Main;
            InitializeComponent();

            BindingContext = _viewModel;
            ToolbarItems.Add(new ToolbarItem("Settings", "settings.png", ShowSettingsPage));
            FloatingActionButtonAdd.IsEnabled = false;
            FloatingActionButtonAdd.Clicked = AddButtonClicked;
        }

        private async void ShowSettingsPage()
        {
            var settingsPage = new SettingsPage();
            await Navigation.PushModalAsync(settingsPage, true).ConfigureAwait(true);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync().ConfigureAwait(true);
            if (string.IsNullOrEmpty(_viewModel.MyPlayer?.Id))
            {
                ShowSettingsPage();
            }
            else
            {
                FloatingActionButtonAdd.IsEnabled = true;
            }
        }

        private async void AddButtonClicked(object sender, EventArgs e)
        {
            var newGamePage = new NewGamePage();
            await Navigation.PushAsync(newGamePage).ConfigureAwait(true);
        }

        private async void SfListView_OnSwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            if (e.SwipeDirection == SwipeDirection.Right && e.SwipeOffset > 200)
            {
                var item = e.ItemData as Game;
                await _viewModel.HideGameAsync(item);
                GamesView.ResetSwipe(true);
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

        private async void GamesView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.ItemData is Game game)
            {
                if (game.GameStatus < GameStatus.Started)
                {
                    var newGamePage = new NewGamePage(game.Id);
                    await Navigation.PushAsync(newGamePage).ConfigureAwait(true);
                }
                else
                {
                    var playGamePage = new PlayGamePage(game.Id);
                    await Navigation.PushAsync(playGamePage).ConfigureAwait(true);
                }
            }
        }
    }
}
