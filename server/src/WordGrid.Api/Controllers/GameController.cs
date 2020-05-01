using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WordGrid.Api.Client.Interface;

namespace WordGrid.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        // Initially start with single known game, this will be expanded to support
        // multiple games with many IDs.
        private Guid GameId = new Guid("88b9cc3c-bd93-497e-8f06-03b2d831d020");

        private readonly Core.Models.GameManager _gameManager;
        private readonly AsDto _asDto;

        public GameController(
            Core.Models.GameManager gameManager, 
            AsDto asDto)
        {
            _gameManager = gameManager;
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
            => await _gameManager.CreateNewGameAsync(numberOfRounds, secondsPerRound);

        [HttpGet]
        [Route("next/round")]
        public async Task StartRoundAsync()
            => await _gameManager.StartNextRoundAsync(GameId);


        [HttpGet]
        [Route("finish")]
        public async Task FinishGameAsync()
            => await _gameManager.FinishGameAsync(GameId);
    }
}
