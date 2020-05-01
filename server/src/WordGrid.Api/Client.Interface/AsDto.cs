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
                GameState = AsStateDto(game.State),
                CurrentRound = game.NumberOfPlayedRounds,
                RoundsToPlay = game.RoundsToPlay,
                GridSize = game.Grid.GridSize,
                Grid = AsGridDto(game.Grid),
                RoundTimeRemaining = AsRemaining(game.RoundExpires),
                CountdownLength = game.SecondsPerRound
            };
        }

        private double AsRemaining(DateTime roundExpires)
        {
            if(roundExpires == null)
                return 0;
                
            var seconds = (roundExpires - DateTime.UtcNow).TotalSeconds;
            if(seconds < 0)
                return 0;

            return seconds;
        }

        private GameStateEnum AsStateDto(State state)
             => state switch 
             {
                 State.NotStarted => GameStateEnum.NotStarted,
                 State.AwaitingNextRound => GameStateEnum.AwaitingNextRound,
                 State.ShakingBoard => GameStateEnum.ShakingBoard,
                 State.RoundInProgress => GameStateEnum.RoundInProgress,
                 State.Finished => GameStateEnum.Finished,
                 _ => throw new NotSupportedException("Mapping to state not supported.")
             };

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