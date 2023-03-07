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
                throw new ArgumentNullException(nameof(info));

            TimeZone = (TimeZone) info.GetValue("TimeZone", typeof(TimeZone));
            Amount = info.GetDecimal("Amount");
        }

        public TimeZone TimeZone { get; set; }
        public decimal Amount { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue("TimeZone", TimeZone, typeof(TimeZone));
            info.AddValue("Amount", Amount, typeof(decimal));
        }


        private bool Equals(TimeZoneAmount other)
            => Amount == other.Amount && TimeZone == other.TimeZone;

        public override bool Equals(object obj)
            => ReferenceEquals(this, obj) || obj is TimeZoneAmount other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Amount, TimeZone);
    }
}