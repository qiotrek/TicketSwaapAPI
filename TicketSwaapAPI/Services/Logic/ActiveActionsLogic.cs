using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TicketSwaapAPI.Models.ViewModels;
using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Logic
{
    public interface IActiveActionsLogic
    {
        Task<ActiveActionModel> AddNewOffert(string id, string place, string sector, string userId,string? mainOffertId);
        Task<ActiveActionModel> DeleteOffert(string actionId, string offertId, string userId);
        Task<ActiveActionViewModel> GetActiveAction(string id);
        Task<List<ActiveActionModel>> GetActiveActions(string[]actionIds=null);
        Task<List<string>> GetUserFavoritesActions(string userId);
        Task<bool> UpdateUserFavoriteAction(string userId, string actionId);
    }

    public class ActiveActionsLogic : IActiveActionsLogic
    {
        private readonly IActiveActionsRepository _activeActionsRepository;
        private readonly IOffertRepository _offertRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ActiveActionsLogic> _logger;
        private readonly IUserNotificationService _userNotificationService;

        public ActiveActionsLogic(IActiveActionsRepository activeActionsRepository, IOffertRepository offertRepository, IUserRepository userRepository, IConfiguration configuration, ILogger<ActiveActionsLogic> logger, IUserNotificationService userNotificationService)
        {
            _activeActionsRepository = activeActionsRepository;
            _offertRepository = offertRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
            _userNotificationService = userNotificationService;
        }

        public async Task<ActiveActionViewModel> GetActiveAction(string id)
        {
            ActiveActionModel action = await _activeActionsRepository.Get(id);
            if (action != null && action.Id == id)
            {
                List<OffertModel> offerts = new List<OffertModel>();
                foreach (var offertId in action.Offerts)
                {
                    OffertModel offert = _offertRepository.Get(offertId).Result;
                    if (offert != null)
                    {
                        offerts.Add(offert);
                    }
                }
                return ActiveActionViewModel.ConvertToViewModel(action, offerts);
            }
            else
            {
                return new ActiveActionViewModel();
            }
        }

        public async Task<List<ActiveActionModel>> GetActiveActions(string[] actionIds = null)
        {
            List<ActiveActionModel> actions=await _activeActionsRepository.GetList();
            List<ActiveActionModel> result =new List<ActiveActionModel>();
            if (actionIds != null)
            {
                foreach (var action in actions)
                {
                    if (actionIds.Contains(action.Id))
                    {
                        result.Add(action);
                    }
                }
                return result;
            }
            return actions;
        }

        public async Task<List<string>> GetUserFavoritesActions(string userId)
        {
            UserModel user = await _userRepository.Get(userId);
            return user.FavActions;
        }

        public async Task<bool> UpdateUserFavoriteAction(string userId, string actionId)
        {
            UserModel user = await _userRepository.Get(userId);
            if (user.FavActions == null)
            {
                user.FavActions = new List<string>();
            }
            if (user.FavActions.Count()>0&& user.FavActions.Exists(v => v == actionId))
            {
                user.FavActions.Remove(actionId);
            }
            else
            {
                user.FavActions.Add(actionId);
            }
            var updateResult = await _userRepository.Update(user);
            return (updateResult != null) ? true : false ;
        }
        public async Task<ActiveActionModel> AddNewOffert(string id,string place,string sector,string userId,string? mainOffertId)
        {
            ActiveActionModel action = await _activeActionsRepository.Get(id);
            if (action==null)
            {
                throw new ArgumentException("Brak akcji o wskazanym Id");
            }
            string ofertId = $"{id}_{sector}_{place}";

            OffertModel offert = new OffertModel
            {
                Id = ofertId,
                EventId = id,
                Place = place,
                Sector = sector,
                CreateUser = userId,
                IntrestedOfferts = []
            };
            OffertModel offertCheck = await _offertRepository.Get(ofertId);

            if (offertCheck == null)
            {
                await _offertRepository.Set(offert);
                action.Offerts.Add(offert.Id);
                await _activeActionsRepository.Set(action);
                UserModel user = await _userRepository.Get(userId);
                if (user.Offers == null)
                {
                    user.Offers = new List<string>();
                }
                user.Offers.Add(offert.Id);
                await _userRepository.Update(user);
            }

                 

            if (mainOffertId!=null&&mainOffertId.Length>0)
            {
                OffertModel mainOffert = await _offertRepository.Get(mainOffertId);
                mainOffert.IntrestedOfferts.Add(offert.Id);
                await _offertRepository.Set(mainOffert);
                await _userNotificationService.GetNotification(mainOffert.CreateUser, NotificationType.NewOffert, action, offert);

            }

            return action;            
        }

        public async Task<ActiveActionModel> DeleteOffert(string actionId, string offertId, string userId)
        {
            ActiveActionModel action = await _activeActionsRepository.Get(actionId);
            OffertModel offert = await _offertRepository.Get(offertId);
            if (action == null)
            {
                throw new ArgumentException("Brak akcji o wskazanym Id");
            }
            if (offert == null)
            {
                throw new ArgumentException("Brak oferty o wskazanym Id");
            }
            string offertToRemove = action.Offerts.Find(offert => offert == offertId);
            action.Offerts.Remove(offertToRemove);

            UserModel user = await _userRepository.Get(userId);
            user.Offers.Remove(offertToRemove);
            await _userRepository.Update(user);
            bool res = await _activeActionsRepository.Set(action);

            await _offertRepository.Delete(offert);
            if (res)
            {
                return action;
            }
            else
            {
                throw new ArgumentException("Coś poszło nie tak");
            }
        }

    }
}
