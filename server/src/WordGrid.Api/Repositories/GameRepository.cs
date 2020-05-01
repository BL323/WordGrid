using System;
using Microsoft.Extensions.Caching.Memory;
using WordGrid.Core.Models;
using WordGrid.Core.Repositories;

namespace WordGrid.Api.Respositories 
{
    /// <summary>
    /// Provides a simple implementation for game repository. 
    /// Initially this will use an in memory cache but could be expanded to use 
    /// a persistence mechanism.
    /// </summary>
    public class GameRepository : IGameRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        ///  Initialises a new instance of the <see cref="GameRepository" /> class.
        /// </summary>
        /// <param name="memoryCache">An in memory storage mechanism for running games.</param>
        public GameRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Adds a game to the repository.
        /// </summary>
        /// <param name="game"></param>
        public void AddGame(Game game) => _memoryCache.Set(game.ID, game);

        /// <summary>
        /// Finds a game by it's ID.
        /// </summary>
        /// <param name="id">The unique ID of the game to find.</param>
        public Game FindGame(Guid id)
        {
            if(_memoryCache.TryGetValue(id, out Game game))
                return game;

            return null;
        }

        /// <summary>
        /// Removes a game by it's ID.
        /// </summary>
        public void RemoveGame(Guid id)
        {
            _memoryCache.Remove(id);
        }
    }
}