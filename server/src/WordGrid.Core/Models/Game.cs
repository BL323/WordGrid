using System;

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

        /// <summary>
        /// Initialises a new instance of the <see cref="GameState" /> class.
        /// </summary>
        internal Game(
            Grid grid, 
            int roundsToPlay = 5,
            int secondsPerRound = 90) 
        {
            // when supporting multiple games a new Guid should be generated each time.
            ID = new Guid("88b9cc3c-bd93-497e-8f06-03b2d831d020"); //Guid.NewGuid();
            State = State.AwaitingNextRound;
            Grid = grid;
            RoundsToPlay = roundsToPlay;
            SecondsPerRound = secondsPerRound;
        }

        /// <summary>
        /// Begins a new round within a game.
        /// </summary>
        public void NextRound()
        {
            if(NumberOfPlayedRounds == RoundsToPlay)
                return;

            NumberOfPlayedRounds++;
            State = State.RoundInProgress;
        }

        /// <summary>
        /// Shuffles all the dice on the grid.
        /// </summary>
        public void ShuffleBoard()
        {
            Grid.ShuffleAndRoll();
            State = State.RoundInProgress;
        }

        /// <summary>
        /// Sets the round time limit by adding 1 mintue to existing time.
        /// </summary>
        public void SetCountdown()
        {
            this.RoundExpires = DateTime.UtcNow.AddSeconds(SecondsPerRound);
        }

        /// <summary>
        /// Finishes the game.
        /// </summary>
        public void FinishGame()
        {
            State = State.Finished;
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