using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using Volvo.CongestionTax.Application.Core.Behaviour;
using Volvo.CongestionTax.Application.Core.Exceptions;
using Volvo.CongestionTax.Application.Queries;
using Xunit;

namespace Volvo.CongestionTax.Application.UnitTests
{
    public class ValidationBehaviorTests
    {
        [Fact]
        public void ShouldValidationBehaviourThrowExceptionWhenCommandIsInvalid()
        {
            var requestValidation = new Mock<ValidationBehaviour<CalculateCongestionTaxQuery,
                CalculateCongestionTaxQueryResult>>(new CalculateCongestionTaxCommandValidator());

            Func<Task<CalculateCongestionTaxQueryResult>> task = async () => await requestValidation.Object.Handle(
                new CalculateCongestionTaxQuery
                {
                    CountryCode = "",
                    City = "Gothenburg",
                    VehicleType = "Car",
                    PassagesTimes = new List<DateTime>
                    {
                        DateTime.Today
                    }
                }, () => Task.Factory.StartNew(() => new CalculateCongestionTaxQueryResult()), 
                new CancellationToken()
                );

            task.Should().ThrowAsync<ValidationException>();
        }
    }
}