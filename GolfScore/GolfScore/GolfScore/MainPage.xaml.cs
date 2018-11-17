using System;
using TeeScore.Pages;
using TeeScore.ViewModels;
using Xamarin.Forms;

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
            ToolbarItems.Add(new ToolbarItem("Settings","settings.png",()=> ShowSettingsPage()));
            FloatingActionButtonAdd.Clicked = (o, args) => AddButtonClicked(o, args);
        }

        private async void ShowSettingsPage()
        {
            var settingsPage = new SettingsPage();
            await Navigation.PushModalAsync(settingsPage,true);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.Load();
            if (string.IsNullOrEmpty(_viewModel.MyPlayer.Id))
            {
                ShowSettingsPage();
            }
        }

        private async void AddButtonClicked(object sender, EventArgs e)
        {
            var newGamePage = new NewGamePage();
            await Navigation.PushAsync(newGamePage);
        }
    }
}
