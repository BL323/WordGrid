using WordGrid.Core.Models;

namespace WordGrid.Core.Domain
{
    public class GameFinishedEvent : IDomainEvent 
    {
        public Game Game { get; }

        public GameFinishedEvent(Game game) => Game = game;
    }
}