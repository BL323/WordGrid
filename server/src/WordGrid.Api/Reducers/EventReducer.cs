using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using server.Hubs;
using WordGrid.Api.Client.Interface;
using WordGrid.Core.Domain;

namespace WordGrid.Api.Reducers 
{
    public class EventReducer : IEventPublisher
    {
        private readonly IHubContext<GameHub> _gameHub;
        private readonly AsDto _asDto;

        /// <summary>
        /// Initialises a new instance of the <see cref="EventReducer" /> class.
        /// </summary>
        /// <param name="gameHub">The game hub used for notifications.</param>
        /// <param name="asDto">The entity DTO converter.</param>
        public EventReducer(IHubContext<GameHub> gameHub, AsDto asDto) 
        {
            _gameHub = gameHub;
            _asDto = asDto;
        }

        /// <summary>
        /// Publishes domain events to consuming clients.
        /// </summary>
        public async Task PublishAsync(IDomainEvent @event)
        {
            switch(@event)
            {
                case GameCreatedEvent createdEvent:
                    await HandleCreatedEventAsync(createdEvent);
                    break;
                case StartedShakingBoardEvent shakeBoardEvent:
                    await HandleShakingBoardEvent(shakeBoardEvent);
                    break;
                case StartedNextRoundEvent nextRoundEvent:
                    await HandleNextRoundNextAsync(nextRoundEvent);
                    break;
                case GameFinishedEvent finishedEvent:
                    await HandleFinishedEventAsync(finishedEvent);
                    break;
            }
        }
   
        private async Task HandleCreatedEventAsync(GameCreatedEvent createdEvent)
            => await _gameHub.Clients.All.SendAsync("GameCreatedAsync", _asDto.Game(createdEvent.Game));

        private async Task HandleShakingBoardEvent(StartedShakingBoardEvent shakeBoardEvent)
            => await _gameHub.Clients.All.SendAsync("ShakingBoardAsync", _asDto.Game(shakeBoardEvent.Game));

        private async Task HandleNextRoundNextAsync(StartedNextRoundEvent nextRoundEvent) 
            => await _gameHub.Clients.All.SendAsync("NextRoundAsync", _asDto.Game(nextRoundEvent.Game));     

        private async Task HandleFinishedEventAsync(GameFinishedEvent finishedEvent)
            => await _gameHub.Clients.All.SendAsync("GameFinishedAsync", _asDto.Game(finishedEvent.Game));
    }
}