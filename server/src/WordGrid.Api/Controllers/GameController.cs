using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using server.Hubs;
using WordGrid.Core.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<GameHub> _gameHub;

        public GameController(IHubContext<GameHub> gameHub)
        {
            _gameHub = gameHub;
        }

        [HttpGet]
        [Route("state")]
        public async Task<int> GetGameStateAsync()
        {
            await Task.CompletedTask;
            return 0;
        }

        [HttpGet]
        [Route("start/round")]
        public async Task StartRoundAsync()
        {
            await Task.CompletedTask;
            // await _gameHub.Clients.All.SendAsync("GameStateUpdatedAsync", _state);
        }
    }
}
