using System;
using System.Runtime.Serialization;

namespace Volvo.CongestionTax.Domain.Core
{
    [Serializable]
    public class TimeZone : ISerializable
    {
        public TimeZone(Time from, Time to)
        {
            From = from;
            To = to;
        }

        protected TimeZone(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            From = (Time) info.GetValue(nameof(From), typeof(Time));
            To = (Time) info.GetValue(nameof(To), typeof(Time));
        }

        public Time From { get; }

        public Time To { get; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(From), From, typeof(Time));
            info.AddValue(nameof(To), To, typeof(Time));
        }

        public bool IsInTimeZone(DateTime dateTime)
        {
            return IsInTimeZone(new Time(dateTime.Hour, dateTime.Minute, dateTime.Second));
        }

        public bool IsInTimeZone(Time time)
        {
            return time >= From && time <= To;
        }
    }
}