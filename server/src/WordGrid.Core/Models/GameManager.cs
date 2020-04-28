using System;
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

        /// <summary>
        /// Initialises a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        /// <param name="gameRepository">Repository to find a specific game.</param>
        public GameManager(IGameRepository gameRepository, GridFactory gridFactory)
        {
            _gameRepository = gameRepository;
            _gridFactory = gridFactory;
        }

        public Game GetGame(Guid id) 
        {
            var game = _gameRepository.FindGame(id);
            return game;
        }

        public Game CreateNewGame()
        {
            // use default options
            var diceGrid = _gridFactory.BuildWithClassicDice();
            var game = new Game(diceGrid);
            _gameRepository.AddGame(game);
            return game;
        }
    }
}