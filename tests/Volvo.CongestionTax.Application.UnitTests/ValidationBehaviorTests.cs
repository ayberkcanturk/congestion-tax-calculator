using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Volvo.Application.SharedKernel.Behaviour;
using Volvo.Application.SharedKernel.Exceptions;
using Volvo.CongestionTax.Application.Commands;
using Xunit;
using Xunit.Sdk;

namespace Volvo.CongestionTax.Application.UnitTests
{
    public class ValidationBehaviorTests
    {
        [Fact]
        public void ShouldValidationBehaviourThrowExceptionWhenCommandIsInvalid()
        {
            var requestValidation = new Mock<ValidationBehaviour<CalculateCongestionTaxCommand, 
                CalculateCongestionTaxCommandResult>>(new CalculateCongestionTaxCommandValidator());
                
            Func<Task<CalculateCongestionTaxCommandResult>> task = async () => await requestValidation.Object.Handle(new CalculateCongestionTaxCommand
            {
                CountryCode = "",
                City = "Gothenburg",
                VehicleType = "Car",
                PassagesTimes = new List<DateTime>
                {
                    DateTime.Today
                }
            }, new CancellationToken(), () => Task.Factory.StartNew(()=> new CalculateCongestionTaxCommandResult()));

            task.Should().ThrowAsync<ValidationException>();
        }
    }
}
