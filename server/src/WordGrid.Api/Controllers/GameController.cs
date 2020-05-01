using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using server.Hubs;
using WordGrid.Api.Client.Interface;

namespace WordGrid.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private Guid GameId = new Guid("88b9cc3c-bd93-497e-8f06-03b2d831d020");

        private readonly Core.Models.GameManager _gameManager;
        private readonly IHubContext<GameHub> _gameHub;
        private readonly AsDto _asDto;

        public GameController(
            Core.Models.GameManager gameManager, 
            IHubContext<GameHub> gameHub,
            AsDto asDto)
        {
            _gameManager = gameManager;
            _gameHub = gameHub;
            _asDto = asDto;
        }

        [HttpGet]
        [Route("state")]
        public async Task<Client.Interface.Game> GetGameAsync()
        {
            await Task.CompletedTask;
            var game = _gameManager.GetGame(GameId);
            return _asDto.Game(game);
        }

        [HttpGet]
        [Route("create")]
        public async Task CreateNewGameAsync(int numberOfRounds, int secondsPerRound)
        {
            var game = _gameManager.CreateNewGame(numberOfRounds, secondsPerRound);
            // This should be moved into a domain event reaction to subscribers for the game ID.
            await _gameHub.Clients.All.SendAsync("GameCreatedAsync", _asDto.Game(game));
        }

        [HttpGet]
        [Route("next/round")]
        public async Task StartRoundAsync()
        {
            await Task.CompletedTask;

            var game = _gameManager.GetGame(GameId);
            if(game == null)
                return;

            game.NextRound();

            // shuffling board
            for(var i = 0; i < 5; i++)
            {
                game.ShuffleBoard();
                await _gameHub.Clients.All.SendAsync("ShakingBoardAsync", _asDto.Game(game));
                await Task.Delay(250);
            }

            game.SetCountdown();

            // This should be moved into a domain event reaction to subscribers for the game ID.
            await _gameHub.Clients.All.SendAsync("NextRoundAsync", _asDto.Game(game));
        }

        [HttpGet]
        [Route("finish")]
        public async Task FinishGameAsync()
        {
            await Task.CompletedTask;

            var game = _gameManager.GetGame(GameId);
            if(game == null)
                return;
                 
            game.FinishGame();

            _gameManager.RemoveCompletedGame(game.ID);

            // This should be moved into a domain event reaction to subscribers for the game ID.
            await _gameHub.Clients.All.SendAsync("GameFinishedAsync", _asDto.Game(game));
        }
    }
}
