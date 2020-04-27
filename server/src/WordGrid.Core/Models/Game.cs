namespace WordGrid.Core.Models
{
    public sealed class Game
    {
        /// <summary>
        /// Gets and sets a value indicating if the game is in progress.
        /// </summary>
        public bool InProgress { get; private set; }

        /// <summary>
        /// Gets and sets the number of rounds that have been played.
        /// </summary>
        public int NumberOfPlayedRounds { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GameState" /> class.
        /// </summary>
        internal Game() 
        {
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame() 
        {
            InProgress = true;
        }

        /// <summary>
        /// Begins a new round within a game.
        /// </summary>
        public void BeginRound()
        {
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