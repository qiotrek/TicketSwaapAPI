using TicketSwaapAPI.Models.ViewModels;
using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Logic
{
    public interface IAdminPanelLogic
    {
        Task<List<OffertViewModel>> GetMyOfferts(string userId);
    }

    public class AdminPanelLogic : IAdminPanelLogic
    {
        private readonly IActiveActionsRepository _activeActionsRepository;
        private readonly IOffertRepository _offertRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminPanelLogic> _logger;
        public AdminPanelLogic(IActiveActionsRepository activeActionsRepository, IOffertRepository offertRepository, IUserRepository userRepository, IConfiguration configuration, ILogger<AdminPanelLogic> logger)
        {
            _activeActionsRepository = activeActionsRepository;
            _offertRepository = offertRepository;
            _userRepository = userRepository;
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
    }
}
