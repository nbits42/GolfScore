using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.DTO;

namespace TeeScore.ViewModels
{
    public class GameViewModel: MyViewModelBase
    {
        public GameViewModel(IDataService dataService, INavigationService navigationService) : base(dataService, navigationService)
        {
        }

        public async Task LoadAsync(string gameId)
        {
            GameDto = await DataService.GetGame(gameId);
        }

        public PlayGameDto GameDto { get; set; }
    }
}
