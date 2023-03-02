using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Volvo.CongestionTax.Application.Commands;
using Volvo.CongestionTax.Application.Core.Behaviour;
using Volvo.CongestionTax.Application.Core.Exceptions;
using Xunit;

namespace Volvo.CongestionTax.Application.UnitTests
{
    public class ValidationBehaviorTests
    {
        [Fact]
        public void ShouldValidationBehaviourThrowExceptionWhenCommandIsInvalid()
        {
            var requestValidation = new Mock<ValidationBehaviour<CalculateCongestionTaxCommand,
                CalculateCongestionTaxCommandResult>>(new CalculateCongestionTaxCommandValidator());

            Func<Task<CalculateCongestionTaxCommandResult>> task = async () => await requestValidation.Object.Handle(
                new CalculateCongestionTaxCommand
                {
                    CountryCode = "",
                    City = "Gothenburg",
                    VehicleType = "Car",
                    PassagesTimes = new List<DateTime>
                    {
                        DateTime.Today
                    }
                }, new CancellationToken(),
                () => Task.Factory.StartNew(() => new CalculateCongestionTaxCommandResult()));

            task.Should().ThrowAsync<ValidationException>();
        }
    }
}