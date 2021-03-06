﻿using System;
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
        public static CreateGamePage GetNextNewGamePage(NewGameDto game, CreateGamePage currentPage)
        {
            var nextPage = CreateGamePage.VenueSelection;
            game.Game.GameStatus = GameStatus.Creating;
            if (game.Venue == null)
            {
                return nextPage;
            }
            nextPage = CreateGamePage.PropertySelection;
            if (game.Game.TeeCount <= 0 || game.Game.InvitedPlayersCount <= 0 || ProceedOnePage(nextPage, currentPage))
            {
                return nextPage;
            }
            nextPage = CreateGamePage.InvitationSelection;

            game.Game.GameStatus = GameStatus.ConfirmationsReceiving;

            if (game.Players.Count < game.Game.InvitedPlayersCount || ProceedOnePage(nextPage, currentPage))
            {
                return nextPage;
            }

            game.Game.GameStatus = GameStatus.ConfirmationsComplete;
            return CreateGamePage.Ready;
        }

        private static bool ProceedOnePage(CreateGamePage nextPage, CreateGamePage currentPage)
        {
            return (int) nextPage == (int)currentPage + 1;
        }
    }
}
