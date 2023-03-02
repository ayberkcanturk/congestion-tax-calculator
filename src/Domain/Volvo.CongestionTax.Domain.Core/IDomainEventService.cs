using System.Threading.Tasks;

namespace Volvo.CongestionTax.Domain.Core
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}