using Google.Cloud.Firestore;

namespace TicketSwaapAPI.StoreModels
{
    [FirestoreData]
    public class OffertModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string EventId { get; set; }

        [FirestoreProperty]
        public string Sector { get; set; }

        [FirestoreProperty]
        public string Place { get; set; }

        [FirestoreProperty]
        public string CreateUser { get; set; }

        [FirestoreProperty]
        public List<string> IntrestedOfferts { get; set; }
        //zamiast to zaminic na swapProposition, i dodawac jako id,place,sek liste
        //lub zmienic model offert i dodac parrent id aby zapobiedz zagniezdzaniu sie akcji

    }
}
