using System;
using WordGrid.Core.Models;

namespace WordGrid.Core.Repositories
{
    /// <summary>
    /// Provides access to running games.
    /// </summary>
    public interface IGameRepository
    {
        /// <summary>
        /// Adds a game to the repository.
        /// </summary>
        void AddGame(Game game);

        /// <summary>
        /// Finds a single game by corresponding ID.
        /// </summary>
        Game FindGame(Guid id);

        /// <summary>
        /// Removes a game, maybe abandonned.
        /// </summary>
        void RemoveGame(Guid id);
    }   
}