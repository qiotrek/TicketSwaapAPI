using Google.Cloud.Firestore;

namespace TicketSwaapAPI.Converters
{
    public class FirestoreDatetimeConverter : IFirestoreConverter<DateTime>
    {
        public object ToFirestore(DateTime value) => new DateTime(((value.Ticks / 10000000) * 10000000), DateTimeKind.Local).ToUniversalTime();

        public DateTime FromFirestore(object value)
        {
            switch (value)
            {
                case Timestamp timestamp: return timestamp.ToDateTime().ToLocalTime();
                case DateTime datetimeValue: return datetimeValue;
                case null: throw new ArgumentNullException(nameof(value));
                default: throw new ArgumentException($"Unexpected data: {value.GetType()}");
            }
        }
    }

}
