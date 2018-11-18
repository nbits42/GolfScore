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
        public static CreateGamePage GetNextNewGamePage(GameDto game, CreateGamePage currentPage)
        {
            var nextPage = CreateGamePage.VenueSelection;
            game.Game.GameStatus = GameStatus.Creating;
            if (game.Venue == null)
            {
                return nextPage;
            }
            nextPage = CreateGamePage.PropertySelection;
            if (game.Game.TeeCount <= 0)
            {
                return nextPage;
            }
            nextPage = CreateGamePage.InvitationSelection;
            if (game.Game.InvitedPlayersCount <= 0)
            {
                return nextPage;
            }
            game.Game.GameStatus = GameStatus.ConfirmationsReceiving;
            nextPage = CreateGamePage.InvitationWaiting;

            if (game.Game.ConnectedPlayersCount < game.Game.InvitedPlayersCount)
            {
                return nextPage;
            }
            game.Game.GameStatus = GameStatus.ConfirmationsComplete;
            return CreateGamePage.Ready;
        }
    }
}
