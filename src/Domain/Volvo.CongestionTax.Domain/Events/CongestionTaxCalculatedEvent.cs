using System;
using System.Collections.Generic;
using MediatR;
using Volvo.Domain.SharedKernel;

namespace Volvo.CongestionTax.Domain.Events
{
    public class CongestionTaxCalculatedEvent : DomainEvent, INotification
    {
        public string City { get; set; }
        public string VehicleType { get; set; }
        public IList<DateTime> PassagesTimes { get; set; }
        public decimal Amount { get; set; }
    }
}