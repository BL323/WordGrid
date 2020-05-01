using WordGrid.Core.Models;

namespace WordGrid.Core.Domain
{
    public class StartedNextRoundEvent : IDomainEvent 
    {
        public Game Game { get; }

        public StartedNextRoundEvent(Game game) => Game = game;
    }
}