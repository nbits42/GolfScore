using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.TabView;
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

            SetToolbaritems();
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


        private void SetToolbaritems()
        {
            ToolbarItems.Clear();
            var infoItem = new ToolbarItem("Info", "info.png", () => ShowInfo());
            var scoreItem = TabView.SelectedIndex > 1
                ? new ToolbarItem("Score", "todo.png", () => ShowToDo())
                : new ToolbarItem("Score", "score.png", () => ShowScore());
            var finishItem = new ToolbarItem("Finish","finish.png", async () => await _vm.Finish());

            ToolbarItems.Add(infoItem);
            ToolbarItems.Add(scoreItem);
            ToolbarItems.Add(finishItem);
        }

        private void ShowScore()
        {
            TabView.SelectedIndex = 2;
        }

        private void ShowToDo()
        {
            TabView.SelectedIndex = 1;
        }

        private void ShowInfo()
        {
            TabView.SelectedIndex = 0;
        }



        private void SfListView_OnSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            if (e.AddedItems.FirstOrDefault() is TeeDto tee)
            {
                _vm.GotoTee(tee);
            }
        }


        private void TabView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetToolbaritems();
        }
    }
}