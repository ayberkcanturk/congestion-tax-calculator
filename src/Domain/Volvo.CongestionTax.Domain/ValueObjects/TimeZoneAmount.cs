using System;
using System.Runtime.Serialization;
using TimeZone = Volvo.CongestionTax.Domain.Core.TimeZone;

namespace Volvo.CongestionTax.Domain.ValueObjects
{
    [Serializable]
    public class TimeZoneAmount : ISerializable
    {
        public TimeZoneAmount()
        {
        }

        protected TimeZoneAmount(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            TimeZone = (TimeZone) info.GetValue("TimeZone", typeof(TimeZone));
            Amount = info.GetDecimal("Amount");
        }

        public TimeZone TimeZone { get; set; }
        public decimal Amount { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            info.AddValue("TimeZone", TimeZone, typeof(TimeZone));
            info.AddValue("Amount", Amount, typeof(decimal));
        }
    }
}