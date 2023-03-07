using System;
using System.Collections.Generic;
using MediatR;

namespace Volvo.CongestionTax.Application.Queries
{
    public class CalculateCongestionTaxQuery : IRequest<CalculateCongestionTaxQueryResult>
    {
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string VehicleType { get; set; }
        public IList<DateTime> PassagesTimes { get; set; }
    }

    public class CalculateCongestionTaxQueryResult
    {
        public decimal Amount { get; set; }
    }
}