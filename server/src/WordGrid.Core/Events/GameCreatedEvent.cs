using WordGrid.Core.Models;

namespace WordGrid.Core.Domain
{
    public class GameCreatedEvent : IDomainEvent 
    {
        public Game Game { get; }

        public GameCreatedEvent(Game game) => Game = game;
    }
}