using System;
using System.Threading.Tasks;
using GlobalContracts.Enumerations;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.PopupLayout;
using TeeScore.Domain;
using TeeScore.Helpers;
using TeeScore.Pages;
using TeeScore.Services;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;
using SwipeDirection = Syncfusion.ListView.XForms.SwipeDirection;

namespace TeeScore
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;
        private readonly SfPopupLayout _gamePopup;

        public MainPage()
        {
            _viewModel = App.IOC.Main;
            InitializeComponent();

            BindingContext = _viewModel;
            ToolbarItems.Add(new ToolbarItem("Settings", "settings.png", ShowSettingsPage));
            FloatingActionButtonAdd.IsVisible = false;
            FloatingActionButtonAdd.IsEnabled = false;
            FloatingActionButtonAdd.Clicked = AddButtonClicked;
            _gamePopup = CreateGamePopup();
        }

        private SfPopupLayout CreateGamePopup() => new SfPopupLayout
        {
            PopupView =
                {
                    Margin = 5,
                    AppearanceMode = AppearanceMode.OneButton,
                    AcceptButtonText = string.Empty,
                    ShowFooter = false,
                    ShowHeader = false,
                    ShowCloseButton = false,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    PopupStyle = new PopupStyle {CornerRadius = 20},
                    AnimationMode = AnimationMode.Fade,
                    HeightRequest = 200,
                    WidthRequest = 200,
                    BackgroundColor = Color.White,
                    ContentTemplate = new DataTemplate(() =>
                    {
                        var stack = new StackLayout
                        {
                            Orientation = StackOrientation.Vertical
                        };

                        var newGameButton = new Button
                        {
                            Text = "New game",
                            Style = StylingService.GetStyle("PopupButtonStyle"),
                        };
                        newGameButton.Clicked+=NewGameButton_Clicked;

                        var joinLabel = new Label
                        {
                            Text = "Join game",
                            Style = StylingService.GetStyle("SmallLabelStyle"),
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(0,5,0,5)
                        };

                        var joinQrGameButton = new Button
                        {
                            Text = "with QR Code",
                            Style = StylingService.GetStyle("PopupButtonStyle"),
                        };
                        joinQrGameButton.Clicked+=JoinQrGameButton_Clicked;

                        var joinInvitationGameButton = new Button
                        {
                            Text = "with Invitiation number",
                            Style = StylingService.GetStyle("PopupButtonStyle"),
                        };
                        joinInvitationGameButton.Clicked+=JoinInvitationGameButton_Clicked;

                        stack.Children.Add(newGameButton);
                        stack.Children.Add(joinLabel);
                        stack.Children.Add(joinQrGameButton);
                        stack.Children.Add(joinInvitationGameButton);
                        return stack;
                    })
                }
        };

        private async void JoinInvitationGameButton_Clicked(object sender, EventArgs e)
        {
            await JoinInvitationGame();
        }

        private async void JoinQrGameButton_Clicked(object sender, EventArgs e)
        {
            await JoinQrGame();
        }

        private async Task JoinQrGame()
        {
            var joinGamePage = new JoinGamePageByQr();
            await Navigation.PushAsync(joinGamePage).ConfigureAwait(true);
        }

        private async Task JoinInvitationGame()
        {
            var joinGamePage = new JoinGamePage();
            await Navigation.PushAsync(joinGamePage).ConfigureAwait(true);
        }

        private async void NewGameButton_Clicked(object sender, EventArgs e)
        {
            await NewGame();
        }

        private async Task NewGame()
        {
            var newGamePage = new NewGamePage();
            await Navigation.PushAsync(newGamePage).ConfigureAwait(true);
        }

        private async void ShowSettingsPage()
        {
            var settingsPage = new SettingsPage();
            await Navigation.PushModalAsync(settingsPage, true).ConfigureAwait(true);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Active = false;
            await _viewModel.LoadAsync().ConfigureAwait(true);
            _viewModel.Active = true;
            if (string.IsNullOrEmpty(_viewModel.MyPlayer?.Id))
            {
                ShowSettingsPage();
            }
            else
            {
                FloatingActionButtonAdd.IsVisible = true;
                FloatingActionButtonAdd.IsEnabled = true;

                if (!string.IsNullOrEmpty(Settings.StartGameId))
                {
                    var gameId = Settings.StartGameId;
                    Settings.StartGameId = string.Empty;
                    await PlayGame(gameId).ConfigureAwait(true);
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.Active = false;
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            _gamePopup.Show();
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
                var refreshedGame = await _viewModel.GetGame(game.Id);
                if (refreshedGame.GameStatus < GameStatus.Started)
                {
                    var newGamePage = new NewGamePage(game.Id);
                    await Navigation.PushAsync(newGamePage).ConfigureAwait(true);
                }
                else
                {
                    await PlayGame(game.Id);
                }
            }
        }

        private async Task PlayGame(string gameId)
        {
            var playGamePage = new PlayGamePage(gameId);
            await Navigation.PushAsync(playGamePage).ConfigureAwait(true);
        }
    }
}
