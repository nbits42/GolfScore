using System;
using Syncfusion.ListView.XForms;
using TeeScore.Domain;
using TeeScore.Pages;
using TeeScore.ViewModels;
using Xamarin.Forms;
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
    }
}
