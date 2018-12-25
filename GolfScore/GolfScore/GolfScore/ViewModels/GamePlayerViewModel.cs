using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;
using INavigationService = TeeScore.Contracts.INavigationService;

namespace TeeScore.ViewModels
{
    public class GamePlayerViewModel : MyViewModelBase
    {
        private readonly IModalDialogService _dialogService;
        private ObservableCollection<PlayerDto> _players;
        private PlayerDto _player;
        private bool _playerFieldsEnabled;
        private bool _isValid;
        private ObservableCollection<PlayerDto> _knownPlayers;
        private PlayerDto _selectedKnownPlayer;
        private string _gameId;

        public GamePlayerViewModel(IDataService dataService, INavigationService navigationService, IModalDialogService dialogService) : base(dataService, navigationService)
        {
            _dialogService = dialogService;
            PropertyChanged += GamePlayerViewModel_PropertyChanged;
        }

        private void GamePlayerViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedKnownPlayer):
                    if (SelectedKnownPlayer == null)
                    {
                        Player.Reset();
                        PlayerFieldsEnabled = true;
                    }
                    else
                    {
                        Player = SelectedKnownPlayer;
                        PlayerFieldsEnabled = false;
                    }

                    break;
            }
        }

        public async Task LoadAsync()
        {
            var knownPlayers = await DataService.GetKnownPlayers(Settings.MyPlayerId);
            if (Players != null)
            {
                KnownPlayers = new ObservableCollection<PlayerDto>(knownPlayers.Where(x => Players.All(p => p.Id != x.Id)).ToList());
            }
            Player = new PlayerDto();
            PlayerFieldsEnabled = true;
        }

        /* =========================================== property: KnownPlayers ====================================== */
        /// <summary>
        /// Sets and gets the KnownPlayers property.
        /// </summary>
        public ObservableCollection<PlayerDto> KnownPlayers
        {
            get => _knownPlayers;
            set
            {
                if (value == _knownPlayers)
                {
                    return;
                }

                _knownPlayers = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: SelectedPlayer ====================================== */
        /// <summary>
        /// Sets and gets the SelectedPlayer property.
        /// </summary>
        public PlayerDto SelectedKnownPlayer
        {
            get => _selectedKnownPlayer;
            set
            {
                if (value == _selectedKnownPlayer)
                {
                    return;
                }

                _selectedKnownPlayer = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== property: Players ====================================== */
        /// <summary>
        /// Sets and gets the Players property.
        /// </summary>
        public ObservableCollection<PlayerDto> Players
        {
            get => _players;
            set
            {
                if (value == _players)
                {
                    return;
                }

                _players = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: GameId ====================================== */
        /// <summary>
        /// Sets and gets the GameId property.
        /// </summary>
        public string GameId
        {
            get => _gameId;
            set
            {
                if (value == _gameId)
                {
                    return;
                }

                _gameId = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== property: Player ====================================== */
        /// <summary>
        /// Sets and gets the Player property.
        /// </summary>
        public PlayerDto Player
        {
            get => _player;
            set
            {
                if (value == _player)
                {
                    return;
                }

                _player = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: PlayerFieldsEnabled ====================================== */
        /// <summary>
        /// Sets and gets the PlayerFieldsEnabled property.
        /// </summary>
        public bool PlayerFieldsEnabled
        {
            get => _playerFieldsEnabled;
            set
            {
                if (value == _playerFieldsEnabled)
                {
                    return;
                }

                _playerFieldsEnabled = value;
                RaisePropertyChanged();
            }
        }

        public async Task<bool> ValidateAndSave()
        {
            var valid = Player.IsValid;

            if (Players.Any(x => x.Name.Equals(Player.Name, StringComparison.OrdinalIgnoreCase) && 
                                 x.Abbreviation.Equals(Player.Abbreviation, StringComparison.OrdinalIgnoreCase)))
            {
                _dialogService.ShowError("This player is already on your game", "Validation error");
                return false;
            }

            if (valid)
            {
                var player = await DataService.SavePlayer(Player);
                var gamePlayer = new GamePlayer
                {
                    GameId = GameId,
                    PlayerId = player.Id
                };
                await DataService.SaveGamePlayer(gamePlayer);
                Players.Add(player);
            }
            return valid;
        }
    }
}
