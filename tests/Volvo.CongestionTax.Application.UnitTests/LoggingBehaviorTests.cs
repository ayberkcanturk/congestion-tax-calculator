using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Volvo.CongestionTax.Application.Core.Behaviour;
using Volvo.CongestionTax.Application.Queries;
using Xunit;

namespace Volvo.CongestionTax.Application.UnitTests
{
    public class LoggingBehaviorTests
    {
        private readonly Mock<ILogger<CalculateCongestionTaxQuery>> _logger;

        public LoggingBehaviorTests()
        {
            _logger = new Mock<ILogger<CalculateCongestionTaxQuery>>();
        }

        [Fact]
        public async Task ShouldLoggingBehaviourCallLogInformationWhenCommandHandled()
        {
            var requestLogger = new Mock<LoggingBehaviour<CalculateCongestionTaxQuery>>(_logger.Object);
            await requestLogger.Object.Process(new CalculateCongestionTaxQuery
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
                (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()));
        }
    }
}