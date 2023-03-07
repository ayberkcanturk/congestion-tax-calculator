using System;
using System.Threading.Tasks;
using FluentAssertions;
using Volvo.CongestionTax.Application.Core.Exceptions;
using Volvo.CongestionTax.Application.IntegrationTests.Fixtures;
using Volvo.CongestionTax.Application.Queries;
using Volvo.CongestionTax.Tests.Common;
using Xunit;

namespace Volvo.CongestionTax.Application.IntegrationTests
{
    public class CalculateCongestionTaxQueryTests : IClassFixture<ServiceCollectionFixture>
    {
        private readonly ServiceCollectionFixture _fixture;

        public CalculateCongestionTaxQueryTests(ServiceCollectionFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(InvalidCommands.CalculateCongestionTaxCommands), MemberType = typeof(InvalidCommands))]
        public void ShouldThrowValidationException(CalculateCongestionTaxQuery query)
        {
            Func<Task<CalculateCongestionTaxQueryResult>> task = async () => await _fixture.SendAsync(query);

            task.Should().ThrowAsync<ValidationException>();
        }
    }
}