using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using TeeScore.Contracts;
using TeeScore.DTO;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeeScore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayGamePage : ContentPage
    {
        private readonly string _gameId;
        private readonly PlayGameViewModel _vm;
        private PageState _pageState = PageState.New;

        public PlayGamePage(string gameId)
        {
            _gameId = gameId;
            InitializeComponent();

            _vm = App.IOC.PlayGame;
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            switch (_pageState)
            {
                case PageState.New:
                    await _vm.LoadAsync(_gameId);
                    break;
                case PageState.Reappearing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SfListView_OnSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            if (e.AddedItems.FirstOrDefault() is TeeDto tee)
            {
                _vm.GotoTee(tee);
            }
        }
    }
}