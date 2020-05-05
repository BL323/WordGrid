using System;
using System.Threading.Tasks;
using Dawn;
using WordGrid.Core.Domain;

namespace WordGrid.Core.Models
{
    public sealed class Game
    {
        /// <summary>
        /// Gets the unique game ID.
        /// </summary>
        public Guid ID { get; }

        /// <summary>
        /// Gets and sets a value indicating the game state.
        /// </summary>
        public State State { get; private set; }

        /// <summary>
        /// Gets and sets the number of rounds that have been played.
        /// </summary>
        public int NumberOfPlayedRounds { get; private set; }

        /// <summary>
        /// Gets the total number of rounds that will be played during the game.
        /// </summary>
        public int RoundsToPlay { get; }

        /// <summary>
        /// Gets the number of seconds per round.
        /// </summary>
        public int SecondsPerRound { get; }

        /// <summary>
        /// Gets the grid with dice.
        /// </summary>
        public Grid Grid { get; }

        /// <summary>
        /// Gets and sets the expiry time of the current round.
        /// </summary>
        public DateTime RoundExpires { get; private set; }

        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initialises a new instance of the <see cref="GameState" /> class.
        /// </summary>
        internal Game(
            Domain.IEventPublisher eventPublisher,
            Grid grid, 
            int roundsToPlay = 5,
            int secondsPerRound = 90) 
        {
            // when supporting multiple games a new Guid should be generated each time.
            ID = new Guid("88b9cc3c-bd93-497e-8f06-03b2d831d020"); //Guid.NewGuid();
            State = State.AwaitingNextRound;
            Grid = grid;
            RoundsToPlay = Guard.Argument(roundsToPlay, nameof(roundsToPlay))
                                .GreaterThan(0)
                                .LessThan(16)
                                .Value;
            SecondsPerRound = Guard.Argument(secondsPerRound, nameof(secondsPerRound))
                                .GreaterThan(29)
                                .LessThan(301)
                                .Value;
            ;
            _eventPublisher = Guard.Argument(eventPublisher, nameof(eventPublisher))
                                .NotNull()
                                .Value;
        }

        /// <summary>
        /// Initialises a new game.
        /// </summary>
        internal async Task InitialiseNewGameAsync()
            => await _eventPublisher.PublishAsync(new GameCreatedEvent(this));

        /// <summary>
        /// Starts the next round of the game.
        /// </summary>
        internal async Task NextRoundAsync()
        {
            if(NumberOfPlayedRounds == RoundsToPlay)
                return;

            NumberOfPlayedRounds++;
            State = State.RoundInProgress;

            // shuffling board
            for(var i = 0; i < 5; i++)
            {
                await ShuffleBoardAsync();
                await Task.Delay(250);
            }

            await SetCountdownAsync();
        }

        /// <summary>
        /// Finishes the game.
        /// </summary>
        internal async Task FinishGameAsync()
        {
            State = State.Finished;
            await _eventPublisher.PublishAsync(new GameFinishedEvent(this));
        }

        private async Task ShuffleBoardAsync()
        {
            Grid.ShuffleAndRoll();
            State = State.RoundInProgress;
            await _eventPublisher.PublishAsync(new StartedShakingBoardEvent(this));
        }

        private async Task SetCountdownAsync()
        {
            RoundExpires = DateTime.UtcNow.AddSeconds(SecondsPerRound);
            await _eventPublisher.PublishAsync(new StartedNextRoundEvent(this));
        }
    }

    public enum State 
    {
        NotStarted,        
        AwaitingNextRound,
        ShakingBoard,
        RoundInProgress,
        Finished
    }
}