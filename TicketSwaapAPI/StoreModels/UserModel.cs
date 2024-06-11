

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
}
