using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Volvo.Application.SharedKernel.Behaviour;
using Volvo.CongestionTax.Application.Commands;
using Xunit;

namespace Volvo.CongestionTax.Application.UnitTests
{
    public class LoggingBehaviorTests
    {
        private readonly Mock<ILogger<CalculateCongestionTaxCommand>> _logger;

        public LoggingBehaviorTests()
        {
            _logger = new Mock<ILogger<CalculateCongestionTaxCommand>>();
        }

        [Fact]
        public async Task ShouldLoggingBehaviourCallLogInformationWhenCommandHandled()
        {
            var requestLogger = new Mock<LoggingBehaviour<CalculateCongestionTaxCommand>>(_logger.Object);
            await requestLogger.Object.Process(new CalculateCongestionTaxCommand
            {
                CountryCode = "SE",
                City = "Gothenburg",
                VehicleType = "Car",
                PassagesTimes = new List<DateTime>
                {
                    DateTime.Today
                }
            }, new CancellationToken());

            _logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }
    }
}
