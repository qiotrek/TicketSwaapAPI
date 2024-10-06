using Google.Cloud.Firestore;
using TicketSwaapAPI.Converters;

namespace TicketSwaapAPI.StoreModels
{
    [FirestoreData]
    public class ProblemsAndQuestionsModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string userId { get; set; }

        [FirestoreProperty]
        public string Title { get; set; }

        [FirestoreProperty]
        public string Message { get; set; }

        [FirestoreProperty]
        public int Status { get; set; }

        [FirestoreProperty]
        public string Answer { get; set; }


        [FirestoreProperty(ConverterType = typeof(FirestoreDatetimeConverter))]
        public DateTime CreateDate { get; set; }
    }
}
