using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using GolfScore.Pages;
using GolfScore.ViewModels;
using Xamarin.Forms;

namespace GolfScore
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
    }
}
