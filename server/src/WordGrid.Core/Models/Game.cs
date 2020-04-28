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
        /// Gets and sets a value indicating if the game is in progress.
        /// </summary>
        public bool InProgress { get; private set; }

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
        /// Initialises a new instance of the <see cref="GameState" /> class.
        /// </summary>
        internal Game(
            Grid grid, 
            int roundsToPlay = 5,
            int secondsPerRound = 30) 
        {
            // when supporting multiple games a new Guid should be generated each time.
            ID = new Guid("88b9cc3c-bd93-497e-8f06-03b2d831d020"); //Guid.NewGuid();
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

            Grid.ShuffleAndRoll();

            NumberOfPlayedRounds++;
        }

        /// <summary>
        /// Finishes the game.
        /// </summary>
        public void FinishGame()
        {
            InProgress = false;
        }
    }
}