using System;
using System.Threading.Tasks;
using FluentAssertions;
using Volvo.Application.SharedKernel.Exceptions;
using Volvo.CongestionTax.Application.Commands;
using Volvo.CongestionTax.Application.IntegrationTests.Fixtures;
using Volvo.CongestionTax.Tests.SharedKernel;
using Xunit;

namespace Volvo.CongestionTax.Application.IntegrationTests
{
    public class CalculateCongestionTaxCommandTests : IClassFixture<ServiceCollectionFixture>
    {
        private readonly ServiceCollectionFixture _fixture;

        public CalculateCongestionTaxCommandTests(ServiceCollectionFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(InvalidCommands.CalculateCongestionTaxCommands), MemberType = typeof(InvalidCommands))]
        public void ShouldThrowValidationException(CalculateCongestionTaxCommand command)
        {
            Func<Task<CalculateCongestionTaxCommandResult>> task = async () => await _fixture.SendAsync(command);

            task.Should().ThrowAsync<ValidationException>();
        }
    }
}
