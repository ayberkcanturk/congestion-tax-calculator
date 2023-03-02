using System.Collections.Generic;

namespace Volvo.CongestionTax.Domain.Core
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}