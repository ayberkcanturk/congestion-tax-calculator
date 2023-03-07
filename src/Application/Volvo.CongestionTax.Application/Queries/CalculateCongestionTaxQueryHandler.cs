using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Volvo.CongestionTax.Domain.Services;

namespace Volvo.CongestionTax.Application.Queries
{
    public class
        CalculateCongestionTaxQueryHandler : IRequestHandler<CalculateCongestionTaxQuery,
            CalculateCongestionTaxQueryResult>
    {
        private readonly ICongestionTaxService _congestionTaxService;

        public CalculateCongestionTaxQueryHandler(ICongestionTaxService congestionTaxService)
        {
            _congestionTaxService = congestionTaxService;
        }

        public async Task<CalculateCongestionTaxQueryResult> Handle(CalculateCongestionTaxQuery request,
            CancellationToken cancellationToken)
        {
            var amount = await _congestionTaxService.CalculateAsync(request.CountryCode,
                request.City, request.VehicleType, request.PassagesTimes, cancellationToken);

            return new CalculateCongestionTaxQueryResult
            {
                Amount = amount
            };
        }
    }
}