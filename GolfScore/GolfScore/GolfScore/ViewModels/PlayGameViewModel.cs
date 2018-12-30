using System;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;
using TeeScore.Helpers;

namespace TeeScore.ViewModels
{
    public class PlayGameViewModel : MyViewModelBase
    {
        private readonly IDialogService _dialogService;
        private ObservableCollection<TeeDto> _tees;
        private TeeDto _currentTee;
        private PlayerDto _currentPlayer;
        private PlayGameDto _game;
        private ObservableCollection<ScoreDto> _scores;
        private ScoreDto _currentScore;
        private RelayCommand _backCommand;
        private RelayCommand _nextCommand;
        private bool _isBackEnabled;
        private bool _isNextEnabled;
        private string _title;
        private ObservableCollection<TotalScoreDto> _totalScores;
        private RelayCommand _scoreUpCommand;
        private RelayCommand _scoreDownCommand;
        private bool _scoringIsEnabled;

        public PlayGameViewModel(IDataService dataService, INavigationService navigationService, IDialogService dialogService) : base(dataService, navigationService)
        {
            _dialogService = dialogService;
        }

        public async Task LoadAsync(string gameId)
        {
            var gameDto = await DataService.GetGame(gameId);

            Tees = new ObservableCollection<TeeDto>(gameDto.Tees.OrderBy(x => x.Number));
            foreach (var teeDto in Tees)
            {
                teeDto.Scores = new ObservableCollection<ScoreDto>(gameDto.Scores.Where(x => x.TeeId == teeDto.Id).OrderBy(x => x.PlayerAbbreviation));
            }

            var scores = gameDto.Scores.OrderBy(x => x.PlayerId).ToList();
            foreach (var scoreDto in scores)
            {
                scoreDto.Player = gameDto.Players.First(x => x.Id == scoreDto.PlayerId);
                scoreDto.TeeNumber = gameDto.Tees.First(x => x.Id == scoreDto.TeeId).Number;
            }

            TotalScores = new ObservableCollection<TotalScoreDto>();
            foreach (var player in gameDto.Players.OrderBy(x => x.Abbreviation))
            {
                TotalScores.Add(new TotalScoreDto
                {
                    PlayerId = player.Id,
                    TotalPutts = scores.Where(x=>x.PlayerId == player.Id).Sum(x=>x.Putts),
                });
            }

            Scores = new ObservableCollection<ScoreDto>(scores.OrderBy(x => x.TeeNumber).ThenBy(x => x.PlayerAbbreviation));
            Game = gameDto;
            CurrentScore = Scores[Game.Game.CurrentTee];
            CurrentScore.ScoreChanged += CurrentScore_ScoreChanged;
            CurrentTee = Tees.First(x => x.Id == CurrentScore.TeeId);
            IsBackEnabled = false;
            IsNextEnabled = true;
            ScoringIsEnabled = Game.Game.FinishedAt == DomainBase.EmptyDate;
            Title = $"{Game.Game.GameType} @ {Game.Venue.Name}";
        }

        private void CurrentScore_ScoreChanged(object sender, System.EventArgs e)
        {
            var totalScore = TotalScores.First(x => x.PlayerId == CurrentScore.PlayerId);
            totalScore.TotalPutts = Scores.Where(x => x.PlayerId == CurrentScore.PlayerId).Sum(x => x.Putts);
        }

        /* =========================================== property: Game ====================================== */
        /// <summary>
        /// Sets and gets the Game property.
        /// </summary>
        public PlayGameDto Game
        {
            get => _game;
            set
            {
                if (value == _game)
                {
                    return;
                }

                _game = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== property: Tees ====================================== */
        /// <summary>
        /// Sets and gets the Tees property.
        /// </summary>
        public ObservableCollection<TeeDto> Tees
        {
            get => _tees;
            set
            {
                if (value == _tees)
                {
                    return;
                }

                _tees = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: Scores ====================================== */
        /// <summary>
        /// Sets and gets the Scores property.
        /// </summary>
        public ObservableCollection<ScoreDto> Scores
        {
            get => _scores;
            set
            {
                if (value == _scores)
                {
                    return;
                }

                _scores = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== property: CurrentTee ====================================== */
        /// <summary>
        /// Sets and gets the CurrentTee property.
        /// </summary>
        public TeeDto CurrentTee
        {
            get => _currentTee;
            set
            {
                if (value == _currentTee)
                {
                    return;
                }

                _currentTee = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: CurrentPlayer ====================================== */
        /// <summary>
        /// Sets and gets the CurrentPlayer property.
        /// </summary>
        public PlayerDto CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (value == _currentPlayer)
                {
                    return;
                }

                _currentPlayer = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: CurrentScore ====================================== */
        /// <summary>
        /// Sets and gets the CurrentScor property.
        /// </summary>
        public ScoreDto CurrentScore
        {
            get => _currentScore;
            set
            {
                if (value == _currentScore)
                {
                    return;
                }

                _currentScore = value;
                RaisePropertyChanged();
            }
        }



        /* =========================================== RelayCommand: BackCommand ====================================== */
        /// <summary>
        /// Executes the Back command.
        /// </summary>
        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand
                       ?? (_backCommand = new RelayCommand(
                           async () => { await Back(); }));
            }
        }

        private async Task Back()
        {
            var currentScoreIx = Scores.IndexOf(CurrentScore);
            Game.Game.CurrentTee = currentScoreIx;
            SaveScore();
            if (currentScoreIx > 0)
            {
                currentScoreIx--;
            }
            else
            {
                if (Game.Game.StartTee > 1)
                {
                    currentScoreIx = Scores.Count - 1;
                }
            }
            CurrentScore.ScoreChanged -= CurrentScore_ScoreChanged;
            CurrentScore = Scores[currentScoreIx];
            CurrentScore.ScoreChanged += CurrentScore_ScoreChanged;

            CurrentTee = Tees.FirstOrDefault(x => x.Id == CurrentScore.TeeId);
            SetButtons(currentScoreIx);
            Settings.CurrentScoreIx = currentScoreIx;
            await Task.Yield();
        }

        private void SaveScore()
        {
            if (Game.Game.StartedAt == DomainBase.EmptyDate)
            {
                Game.Game.StartedAt = DateTime.Now;
            }
            Task.Run(async () => await DataService.SaveGame(Game.Game, true).ConfigureAwait(false));
            Task.Run(async () => await DataService.SaveScore(CurrentScore, true).ConfigureAwait(false));
        }

        /* =========================================== property: IsBackEnabled ====================================== */
        /// <summary>
        /// Sets and gets the IsBackEnabled property.
        /// </summary>
        public bool IsBackEnabled
        {
            get => _isBackEnabled;
            set
            {
                if (value == _isBackEnabled)
                {
                    return;
                }

                _isBackEnabled = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: IsNextEnabled ====================================== */
        /// <summary>
        /// Sets and gets the IsNextEnabled property.
        /// </summary>
        public bool IsNextEnabled
        {
            get => _isNextEnabled;
            set
            {
                if (value == _isNextEnabled)
                {
                    return;
                }

                _isNextEnabled = value;
                RaisePropertyChanged();
            }
        }




        /* =========================================== RelayCommand: NextCommand ====================================== */
        /// <summary>
        /// Executes the Next command.
        /// </summary>
        public RelayCommand NextCommand
        {
            get
            {
                return _nextCommand
                       ?? (_nextCommand = new RelayCommand(
                           async () => { await Next(); }));
            }
        }

        private async Task Next()
        {
            var currentScoreIx = Scores.IndexOf(CurrentScore);
            Game.Game.CurrentTee = currentScoreIx;
            SaveScore();
            if (currentScoreIx < Scores.Count - 1)
            {
                currentScoreIx++;
            }
            else
            {
                if (Game.Game.StartTee > 1)
                {
                    currentScoreIx = 0;
                }
            }
            CurrentScore.ScoreChanged -= CurrentScore_ScoreChanged;
            SetButtons(currentScoreIx);
            CurrentScore = Scores[currentScoreIx];
            CurrentTee = Tees.FirstOrDefault(x => x.Id == CurrentScore.TeeId);
            CurrentScore.ScoreChanged += CurrentScore_ScoreChanged;
            Settings.CurrentScoreIx = currentScoreIx;
            await Task.Yield();
        }

        private void SetButtons(int currentScoreIx)
        {
            IsNextEnabled = Game.Game.StartTee != 1 || currentScoreIx < Scores.Count - 1;
            IsBackEnabled = Game.Game.StartTee != 1 || currentScoreIx > 0;
        }

        /* =========================================== property: Title ====================================== */
        /// <summary>
        /// Sets and gets the Title property.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                {
                    return;
                }

                _title = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: TotalScores ====================================== */
        /// <summary>
        /// Sets and gets the TotalScores property.
        /// </summary>
        public ObservableCollection<TotalScoreDto> TotalScores
        {
            get => _totalScores;
            set
            {
                if (value == _totalScores)
                {
                    return;
                }

                _totalScores = value;
                RaisePropertyChanged();
            }
        }


        /* =========================================== RelayCommand: ScoreUpCommand ====================================== */
        /// <summary>
        /// Executes the ScoreUp command.
        /// </summary>
        public RelayCommand ScoreUpCommand => _scoreUpCommand
                       ?? (_scoreUpCommand = new RelayCommand(ScoreUp));

        private void ScoreUp()
        {
            CurrentScore.Putts++;
        }

        /* =========================================== RelayCommand: ScoreDownCommand ====================================== */
        /// <summary>
        /// Executes the ScoreUp command.
        /// </summary>
        public RelayCommand ScoreDownCommand => _scoreDownCommand
                       ?? (_scoreDownCommand = new RelayCommand(ScoreDown));

        private void ScoreDown()
        {
            if (CurrentScore.Putts > 0)
            {
                CurrentScore.Putts--;
            }
            
        }

        public void GotoTee(TeeDto tee)
        {
            SaveScore();
            CurrentScore.ScoreChanged -= CurrentScore_ScoreChanged;
            CurrentScore = Scores.First(x => x.TeeId == tee.Id);
            CurrentScore.ScoreChanged += CurrentScore_ScoreChanged;
            var currentScoreIx = Scores.IndexOf(CurrentScore);
            SetButtons(currentScoreIx);
        }

        /* =========================================== property: ScoringIsEnabled ====================================== */
        /// <summary>
        /// Sets and gets the ScoringIsEnabled property.
        /// </summary>
        public bool ScoringIsEnabled
        {
            get => _scoringIsEnabled;
            set
            {
                if (value == _scoringIsEnabled)
                {
                    return;
                }

                _scoringIsEnabled = value;
                RaisePropertyChanged();
            }
        }
        
        public async Task Finish()
        {
            var finish = await _dialogService.ShowMessage("Are you sure you want to finish the game?", "Finish", "Yes", "No");
            if (finish)
            {
                ScoringIsEnabled = false;
                Game.Game.FinishedAt = DateTime.Now;
                SaveScore();
            }
        }
    }
}
