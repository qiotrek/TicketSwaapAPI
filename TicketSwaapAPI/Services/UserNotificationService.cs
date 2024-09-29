using System;
using TicketSwaapAPI.Models.ViewModels;
using TicketSwaapAPI.Services.Infrastructure;
using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services
{
    public enum NotificationType : int
    {
        ActionStart = 1,
        NewOffert = 2,
        OffertAccept = 3,
        ActionExpirationTime = 4,
        ActionEnd = 5,
        OffertReject = 6

    }
    public interface IUserNotificationService 
    {
        public Task<Notification> GetNotification(string userId, NotificationType type, ActiveActionModel action, OffertModel? offert);

    }
    public class UserNotificationService: IUserNotificationService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserNotificationService> _logger;
        private readonly IUserRepository _userRepo;


        private readonly FireStoreService _fsSvc;
        private readonly FirestoreTableNamesConfig _fsTabs;
        public UserNotificationService(IUserRepository userRepo,IConfiguration configuration, ILogger<UserNotificationService> logger, FireStoreService fireStoreService, FirestoreTableNamesConfig firestoreTableNamesConfig) 
        {
            _config = configuration;
            _logger = logger;
            _fsSvc = fireStoreService;
            _fsTabs = firestoreTableNamesConfig;
            _userRepo = userRepo;
        }
        public async Task<Notification> GetNotification(string userId,NotificationType type, ActiveActionModel action,OffertModel? offert)
        {
            Notification notification= new Notification();
            if (type == NotificationType.ActionStart)
            {
                notification.Id = Guid.NewGuid().ToString();
                notification.Title = $"{action.Name}";
                notification.Message = $"Ruszyła wymiana biletów na to wydarzenie. Pośpiesz się!";
                notification.CreateDate = DateTime.Now;
                notification.Url = "";
            }
            if (type == NotificationType.NewOffert)
            {
                notification.Id = Guid.NewGuid().ToString();
                notification.Title = $"{action.Name}";
                notification.Message = $"Ktoś właśnie zaproponował Ci wymianę dla twojego miejsca: {offert.Sector}-{offert.Place}";
                notification.CreateDate = DateTime.Now;
                notification.Url = "";
            }
            if (type == NotificationType.OffertAccept)
            {
                notification.Id = Guid.NewGuid().ToString();
                notification.Title = $"{action.Name}";
                notification.Message = $"Twoja oferta została przyjęta. Twoje aktualne miejsce na to wydarzenie to: {offert.Sector}-{offert.Place} ";
                notification.CreateDate = DateTime.Now;
                notification.Url = "";
            }
            if (type == NotificationType.ActionExpirationTime)
            {
                notification.Id = Guid.NewGuid().ToString();
                notification.Title = $"{action.Name}";
                notification.Message = $"Pozostały tylko 2 dni do zamknięcia wymian. Czas ucieka!";
                notification.CreateDate = DateTime.Now;
                notification.Url = "";
            }
            if (type == NotificationType.ActionEnd)
            {
                notification.Id = Guid.NewGuid().ToString();
                notification.Title = $"{action.Name}";
                notification.Message = $"Wymiany dobiegły końca. Dziękujemy";
                notification.CreateDate = DateTime.Now;
                notification.Url = "";
            }
            if (type == NotificationType.OffertReject)
            {
                notification.Id = Guid.NewGuid().ToString();
                notification.Title = $"{action.Name}";
                notification.Message = $"Niestety prośba o wymianę miejsca: {offert.Sector}-{offert.Place} została odrzucona. Spróbuj ponownie z inną ofertą. ";
                notification.CreateDate = DateTime.Now;
                notification.Url = "";
            }

            AddNotificationForUser(userId,notification);
            return notification;
        }

        public async Task<bool> AddNotificationForUser(string userId,Notification notification)
        {
            UserModel user = await _userRepo.Get(userId);
            if (user == null)
            {
                user.Notifications.Add(notification);
                user.UpdateDate = DateTime.Now;
                user.UpdateLogin = "NOTI";
                await _userRepo.Update(user);
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteNotificationForUser(string userId, string notiId)
        {
            UserModel user = await _userRepo.Get(userId);
            if (user == null)
            {
                Notification notification= user.Notifications.Find(v=>v.Id==notiId);
                user.Notifications.Remove(notification);
                user.UpdateDate = DateTime.Now;
                user.UpdateLogin = "NOTI";
                await _userRepo.Update(user);
                return true;
            }
            return false;
        }
    }
}
