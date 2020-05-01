using System.Threading.Tasks;

namespace WordGrid.Core.Domain
{
    public interface IEventPublisher
    {
        Task PublishAsync(IDomainEvent @event);
    }
}