using System;
using System.Collections.Generic;
using System.Text;
using GlobalContracts.Enumerations;
using TeeScore.Contracts;
using TeeScore.Domain;
using TeeScore.DTO;

namespace TeeScore.Services
{
    public class GameStateService
    {
        public static CreateGamePage GetNextNewGamePage(GameDto game)
        {
            var nextPage = CreateGamePage.VenueSelection;

            if (game.Venue == null)
            {
                return nextPage;
            }

            nextPage = CreateGamePage.PropertySelection;
            if (game.Game.GameType == GameType.None)
            {
                
            }
        }
    }
}
