using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WordGrid.Api.Client.Interface;

namespace server.Hubs
{
    // https://medium.com/@pielegacy/giving-the-valuescontroller-superpowers-with-signalr-core-7d4ff2be9e1c

    /// <summary>
    /// Defines available game events.
    /// </summary>
    public interface IGameClient 
    {
        Task GameCreatedAsync(Game game);
        Task ShakingBoardAsync(Game game);
        Task NextRoundAsync(Game game);
        Task GameFinishedAsync(Game game);
    }

    /// <summary>
    /// Provides real time updates for the game.
    /// </summary>
    public class GameHub : Hub<IGameClient>
    {
        public GameHub()
        {

        }
    }

}