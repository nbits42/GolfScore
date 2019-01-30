using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GlobalContracts.Enumerations;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;
using TeeScore.Translations;

namespace TeeScore.ViewModels
{
    public class JoinGameViewModel: MyViewModelBase
    {
        private readonly IDialogService _dialogService;
        private int _invitationNumber;
        private bool _isWaiting;
        private RelayCommand _joinGameCommand;
        private string _venue;
        private string _gameType;
        private DateTime _gameDate;
        private bool _showGameData;

        public JoinGameViewModel(IDataService dataService, INavigationService navigationService, IDialogService dialogService) : base(dataService, navigationService)
        {
            _dialogService = dialogService;
        }

        /* =========================================== property: InvitationNumber ====================================== */
        /// <summary>
        /// Sets and gets the InvitationNumber property.
        /// </summary>
        public int InvitationNumber
        {
            get => _invitationNumber;
            set
            {
                if (value == _invitationNumber)
                {
                    return;
                }

                _invitationNumber = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: IsWaiting ====================================== */
        /// <summary>
        /// Sets and gets the IsWaiting property.
        /// </summary>
        public bool IsWaiting
        {
            get => _isWaiting;
            set
            {
                if (value == _isWaiting)
                {
                    return;
                }

                _isWaiting = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: Venue ====================================== */
        /// <summary>
        /// Sets and gets the Venue property.
        /// </summary>
        public string Venue
        {
            get => _venue;
            set
            {
                if (value == _venue)
                {
                    return;
                }

                _venue = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: Location ====================================== */
        /// <summary>
        /// Sets and gets the Location property.
        /// </summary>
        public string GameType
        {
            get => _gameType;
            set
            {
                if (value == _gameType)
                {
                    return;
                }

                _gameType = value;
                RaisePropertyChanged();
            }
        }

/* =========================================== property: GameDate ====================================== */
        /// <summary>
        /// Sets and gets the GameDate property.
        /// </summary>
        public DateTime GameDate
        {
            get => _gameDate;
            set
            {
                if (value == _gameDate)
                {
                    return;
                }

                _gameDate = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: ShowGameData ====================================== */
        /// <summary>
        /// Sets and gets the ShowGameData property.
        /// </summary>
        public bool ShowGameData
        {
            get => _showGameData;
            set
            {
                if (value == _showGameData)
                {
                    return;
                }

                _showGameData = value;
                RaisePropertyChanged();
            }
        }




        /* =========================================== RelayCommand: GetGameCommand ====================================== */
        /// <summary>
        /// Executes the GetGame command.
        /// </summary>
        public RelayCommand JoinGameCommand
        {
            get
            {
                return _joinGameCommand
                       ?? (_joinGameCommand = new RelayCommand(
                           async () => { await JoinGame(); }));
            }
        }

        private async Task JoinGame()
        {
            IsWaiting = true;
            var game = await DataService.GetGame(InvitationNumber);
            if (game == null)
            {
                _dialogService.ShowError(Messages.GameNotFoundByInvitationNumber,Labels.title_join_game);
                IsWaiting = false;
                return;
            }

            var players = await DataService.GetPlayersForGame(game.Id);
            if (players.Any(x=>x.Id == Settings.MyPlayerId))
            {
                _dialogService.ShowError(Messages.PlayerAlreadyJoined,Labels.title_join_game);
            }

            var result = await _dialogService.ShowMessage(string.Format(Messages.GameFoundByInvitationNumber,game.GameTypeName,game.VenueName,game.GameDate,Environment.NewLine, "    "),
                Labels.title_join_game, Labels.btn_yes, Labels.btn_no);

            if (!result)
            {
                IsWaiting = false;
                return;
            }

            Venue = game.VenueName;
            GameType = game.GameTypeName;
            GameDate = game.GameDate;
            ShowGameData = true;
            IsWaiting = true;

            await SaveMeAsGamePlayer(game.Id);
        }

        private async Task SaveMeAsGamePlayer(string gameId)
        {
            var gamePlayer = new GamePlayer
            {
                GameId = gameId,
                PlayerId = Settings.MyPlayerId,
                PlayerRole = PlayerRole.Player
            };
            await DataService.SaveGamePlayer(gamePlayer, true);
        }
    }
}
