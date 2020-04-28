using System;
using System.Collections.Generic;
using System.Linq;
using WordGrid.Core.Models;

namespace WordGrid.Api.Client.Interface
{
    public class AsDto 
    {
        public Game Game(WordGrid.Core.Models.Game game)
        {
            if(game == null)
                return AsNotStartedGameDto();

            return AsGameDto(game);
        }

        private Game AsGameDto(Core.Models.Game game)
        {
            return new Game
            {
                ID = game.ID,
                GameState = GameStateEnum.AwaitingNextRound,
                CurrentRound = game.NumberOfPlayedRounds,
                RoundsToPlay = game.RoundsToPlay,
                GridSize = game.Grid.GridSize,
                Grid = AsGridDto(game.Grid),
            };
        }

        private List<Dice> AsGridDto(Grid grid)
        {
            return grid.Dice.Select(x => 
                new Dice{
                    Position = x.Position,
                    Value = x.VisibleFace.Value,
                    Rotation = x.VisibleFace.Rotation,
                    ShouldUnderline = x.VisibleFace.ShouldUnderline
                }
            ).ToList();
        }

        private Game AsNotStartedGameDto()
        {
            return new Game
            {
                GameState = GameStateEnum.NotStarted
            };
        }
    }
}