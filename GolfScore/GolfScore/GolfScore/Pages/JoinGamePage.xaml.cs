using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeeScore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinGamePage : ContentPage
    {
        private readonly JoinGameViewModel _vm;
        public JoinGamePage()
        {
            InitializeComponent();
            _vm = App.IOC.JoinGame;
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.ShowGameData = false;
        }

        private async void StartGame_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync(true);
        }
    }
}