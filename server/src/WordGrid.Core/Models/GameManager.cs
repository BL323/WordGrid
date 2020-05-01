using System;
using System.Threading.Tasks;
using WordGrid.Core.Domain;
using WordGrid.Core.Repositories;

namespace WordGrid.Core.Models
{
    /// <summary>
    /// Provides access to management of games.
    /// </summary>
    public class GameManager 
    {
        private readonly IGameRepository _gameRepository;
        private readonly GridFactory _gridFactory;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initialises a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        /// <param name="gameRepository">Repository to find a specific game.</param>
        public GameManager(
            IGameRepository gameRepository, 
            GridFactory gridFactory,
            IEventPublisher eventPublisher)
        {
            _gameRepository = gameRepository;
            _gridFactory = gridFactory;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Gets the game by it's ID.
        /// </summary>
        /// <param name="id">The unique ID of the game to be returned.</param>
        public Game GetGame(Guid id) 
        {
            var game = _gameRepository.FindGame(id);
            return game;
        }

        /// <summary>
        /// Creates and initialises a new game.
        /// </summary>
        /// <param name="numberOfRounds">The number of rounds for the new game.</param>
        /// <param name="secondsPerRound">The number of seconds per round.</param>
        public async Task CreateNewGameAsync(int numberOfRounds, int secondsPerRound)
        {
            var diceGrid = _gridFactory.BuildWithClassicDice();
            var game = new Game(_eventPublisher, diceGrid, numberOfRounds, secondsPerRound);
            _gameRepository.AddGame(game);
            await game.InitialiseNewGameAsync();
        }

        /// <summary>
        /// Starts the next round of the game.
        /// </summary>
        /// <param name="gameId">The ID of the game to be started.</param>
        public async Task StartNextRoundAsync(Guid gameId)
        {
            var game = GetGame(gameId);
            if(game == null)
                return;

            await game.NextRoundAsync();   
        }

        /// <summary>
        /// Marks a game with corresponding ID as finished.
        /// </summary>
        /// <param name="gameId">The unique ID of game to be finished.</param>
        public async Task FinishGameAsync(Guid gameId)
        {
            var game = GetGame(gameId);
            if(game == null)
                return;
                 
            await game.FinishGameAsync();

            RemoveCompletedGame(game.ID);
        }

        private void RemoveCompletedGame(Guid id)
        {
            _gameRepository.RemoveGame(id);
        }
    }
}