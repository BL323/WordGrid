using WordGrid.Core.Models;

namespace WordGrid.Core.Domain
{
    public class StartedShakingBoardEvent : IDomainEvent 
    {
        public Game Game { get; }

        public StartedShakingBoardEvent(Game game) => Game = game;
    }
}