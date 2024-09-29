using Google.Cloud.Firestore;
using TicketSwaapAPI.Converters;

namespace TicketSwaapAPI.StoreModels
{
    [FirestoreData]
    public class ActiveActionModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty(ConverterType = typeof(FirestoreDatetimeConverter))]
        public DateTime? EventDate { get; set; }

        [FirestoreProperty]
        public List<string> Offerts { get; set; }

        [FirestoreProperty]
        public int Status { get; set; }

        [FirestoreProperty]
        public string Img { get; set; }


        public static ActiveActionModel ConvertToActiveAction(NewActionProposition newAction)
        {
            List<string> offers = new List<string>();
            return new ActiveActionModel
            {
                Id = newAction.Id,
                Name = newAction.Name,
                EventDate = newAction.EventDate,
                Offerts = offers,
                Status=(int)ActionStatus.During,
                Img= newAction.Img
            };
        }
    }


    public enum ActionStatus:int
    {
        Before=1,
        During=2,
        LastDays=3,
        Closed=4

    }
}
