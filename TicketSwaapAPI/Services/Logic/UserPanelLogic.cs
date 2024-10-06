using TicketSwaapAPI.Models.ViewModels;
using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Logic
{
    public interface IUserPanelLogic
    {
        Task<List<OffertViewModel>> GetMyOfferts(string userId);
        Task<List<Notification>> GetMyNotifications(string userId);
        Task<bool> CloseNotification(string userId,string id);
        Task<bool> AddQuestion(string userId, ProblemsAndQuestionsModel model);
        Task<bool> CloseQuestion(string userId, string id);
    }

    public class UserPanelLogic : IUserPanelLogic
    {
        private readonly IActiveActionsRepository _activeActionsRepository;
        private readonly IOffertRepository _offertRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProblemsAndQuestionsRepository _problemsAndQuestionsRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserPanelLogic> _logger;
        public UserPanelLogic(IActiveActionsRepository activeActionsRepository, IOffertRepository offertRepository, IUserRepository userRepository, IProblemsAndQuestionsRepository problemsAndQuestionsRepository, IConfiguration configuration, ILogger<UserPanelLogic> logger)
        {
            _activeActionsRepository = activeActionsRepository;
            _offertRepository = offertRepository;
            _userRepository = userRepository;
            _problemsAndQuestionsRepository=problemsAndQuestionsRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<OffertViewModel>> GetMyOfferts(string userId)
        {

            List<OffertViewModel> result = new List<OffertViewModel>();
            UserModel user = await _userRepository.Get(userId);
            if (user == null)
            {
                throw new ArgumentException("Cos poszlo nie tak! Sprobuj ponownie.");
            }
            foreach (var offertId in user.Offers)
            {
                OffertModel offert = await _offertRepository.Get(offertId);
                if (offert != null)
                {
                    result.Add(OffertViewModel.ConvertToViewModel(offert));
                }

            }
            return result;
        }
        public async Task<List<Notification>> GetMyNotifications(string userId)
        {
            UserModel user = await _userRepository.Get(userId);

            return user?.Notifications??new List<Notification>();
        }
        public async Task<bool> CloseNotification(string userId, string id) 
        {
            bool result = false;
            UserModel user = await _userRepository.Get(userId);
            Notification notification = user.Notifications.Where(v => v.Id == id).First();
            if (notification.Id == id)
            {
                user.Notifications.Remove(notification);
                UserModel userAfterUpdate= await _userRepository.Update(user);
                if (userAfterUpdate != null)
                {
                    result = true;
                }
            }
        
            return result;
        }

        public async Task<bool> AddQuestion(string userId, ProblemsAndQuestionsModel model)
        {
            bool result = false;
            UserModel user = await _userRepository.Get(userId);
            if (user!=null)
            {
                model.Id = Guid.NewGuid().ToString();
                model.userId = userId;
                model.CreateDate = DateTime.Now;
                model.Status = 0;
                ProblemsAndQuestionsModel QaP= await _problemsAndQuestionsRepository.Set(model);
                if (QaP!=null)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> CloseQuestion(string userId, string id)
        {
            bool result = false;
            UserModel user = await _userRepository.Get(userId);
            if (user != null)
            {
                
                ProblemsAndQuestionsModel QaP = await _problemsAndQuestionsRepository.Get(id);
                if (QaP != null)
                {
                    result= await _problemsAndQuestionsRepository.Delete(QaP);
                }
            }

            return result;
        }
    }
}
