using Google.Cloud.Firestore;
using TicketSwaapAPI.Converters;

namespace TicketSwaapAPI.StoreModels
{
    [FirestoreData]
    public class NewActionProposition
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }


        [FirestoreProperty]
        public string Img { get; set; }

        [FirestoreProperty(ConverterType = typeof(FirestoreDatetimeConverter))]
        public DateTime EventDate { get; set; }

        [FirestoreProperty]
        public int IntrestedCount { get; set; }

        [FirestoreProperty]
        public List<string> IntrestedEmails { get; set; }
    }
}
