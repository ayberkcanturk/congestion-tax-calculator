using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Volvo.CongestionTax.Domain.Events;

namespace Volvo.CongestionTax.Application.Events
{
    public class CongestionTaxCalculatedEventHandler : INotificationHandler<CongestionTaxCalculatedEvent>
    {
        private readonly ILogger<CongestionTaxCalculatedEvent> _logger;

        public CongestionTaxCalculatedEventHandler(ILogger<CongestionTaxCalculatedEvent> logger)
        {
            _logger = logger;
        }

        public Task Handle(CongestionTaxCalculatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CongestionTaxCalculatedEvent received: {notification}", notification);

            return Task.CompletedTask;
        }
    }
}