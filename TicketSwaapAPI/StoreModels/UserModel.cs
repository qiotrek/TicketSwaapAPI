

using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TicketSwaapAPI.Converters;
using TicketSwaapAPI.CustomValidation;

namespace TicketSwaapAPI.StoreModels
{
    [FirestoreData]
    public class UserModel
    {
        [Required]
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string? Description { get; set; }

        [FirestoreProperty]
        public List<string> Offers  { get; set; }

        [FirestoreProperty]
        public List<string> FavActions { get; set; }

        [FirestoreProperty]
        public List<Notification> Notifications { get; set; }

        [FirestoreProperty]
        [Required]
        [UserModelRoleValidation]
        public string Role { get; set; }

        [JsonIgnore]
        [FirestoreProperty(ConverterType = typeof(FirestoreDatetimeConverter))]
        public DateTime? CreateDate { get; set; }
        [JsonIgnore]
        [FirestoreProperty]
        public string? CreateLogin { get; set; }

        [JsonIgnore]
        [FirestoreProperty(ConverterType = typeof(FirestoreDatetimeConverter))]
        public DateTime? UpdateDate { get; set; }

        [JsonIgnore]
        [FirestoreProperty]
        public string? UpdateLogin { get; set; }
    }

    [FirestoreData]
    public class Notification
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Title { get; set; }

        [FirestoreProperty]
        public string Message { get; set; }

        [FirestoreProperty]
        public DateTime CreateDate { get; set; }

        [FirestoreProperty]
        public string Url { get; set; }
    }
}
