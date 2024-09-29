using Google.Cloud.Firestore;
using TicketSwaapAPI.Converters;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Models.ViewModels
{
    public class ActiveActionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? EventDate { get; set; }

        public List<OffertViewModel> Offerts { get; set; }

        public int Status { get; set; }

        public string Img { get; set; }

        public static ActiveActionViewModel ConvertToViewModel(ActiveActionModel action,List<OffertModel> offers)
        {
            List<OffertViewModel> offersList = new List< OffertViewModel >();
            foreach (var offert in offers)
            {
                offersList.Add(OffertViewModel.ConvertToViewModel(offert));
            }
            return new ActiveActionViewModel
            {
                Id = action.Id,
                Name = action.Name,
                EventDate = action.EventDate,
                Status = action.Status,
                Img = action.Img,
                Offerts = offersList
            };
        }
    }

    public class OffertViewModel
    {
        public string Id { get; set; }
        public string Sector { get; set; }
        public string Place { get; set; }
        public string CreateUser { get; set; }
        public List<string> IntrestedOfferts { get; set; }

        public static OffertViewModel ConvertToViewModel(OffertModel offert)
        {
            return new OffertViewModel { 
                Id = offert.Id, 
                Sector = offert.Sector, 
                Place = offert.Place, 
                IntrestedOfferts = offert.IntrestedOfferts, 
                CreateUser = offert.CreateUser 
            };
        }
    }
   
}
