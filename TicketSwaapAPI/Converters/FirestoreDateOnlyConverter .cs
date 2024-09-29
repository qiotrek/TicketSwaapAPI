using Google.Cloud.Firestore;

namespace TicketSwaapAPI.Converters
{
    public class FirestoreDateOnlyConverter : IFirestoreConverter<DateOnly>
    {
        public object ToFirestore(DateOnly value) => value.ToString("yyyy-MM-dd");

        public DateOnly FromFirestore(object value)
        {
            switch (value)
            {
                case string stringValue when DateOnly.TryParse(stringValue, out var dateOnlyValue):
                    return dateOnlyValue;
                case null:
                    throw new ArgumentNullException(nameof(value));
                default:
                    throw new ArgumentException($"Unexpected data: {value.GetType()}");
            }
        }
    }
}
