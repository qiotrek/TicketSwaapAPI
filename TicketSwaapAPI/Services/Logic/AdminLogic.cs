using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Logic
{

    public interface IAdminLogic
    {
        Task<bool> AcceptNewAction(string id, string userId);
        Task<List<NewActionProposition>> GetActionPropositions();
        Task<bool> RejectNewAction(string id);
    }
    public class AdminLogic: IAdminLogic
    {
        private readonly IConfiguration _config;
        private readonly ILogger<NewActionPropositionlogic> _logger;
        private readonly INewActionsPropositionRepository _newActionRepository;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IActiveActionsRepository _activeActionsRepository;

        public AdminLogic(IConfiguration config, ILogger<NewActionPropositionlogic> logger, INewActionsPropositionRepository newActionRepository,/* IAdminRepository adminRepository,*/ IActiveActionsRepository activeActionsRepository, IUserNotificationService userNotificationService)
        {
            _config = config;
            _logger = logger;
            _newActionRepository = newActionRepository;
            //_adminRepository = adminRepository;
            _activeActionsRepository = activeActionsRepository;
            _userNotificationService = userNotificationService;
        }

        public async Task<List<NewActionProposition>> GetActionPropositions()
        {
            List<NewActionProposition> actionPropositions =await _newActionRepository.GetActionPropositions();
            return actionPropositions;
        }

        public async Task<bool> AcceptNewAction(string id,string userId)
        {
            NewActionProposition actionProposition = await _newActionRepository.Get(id);
            if (actionProposition != null)
            {
                ActiveActionModel activeAction = ActiveActionModel.ConvertToActiveAction(actionProposition);
                bool setResult = await _activeActionsRepository.Set(activeAction);
                if (setResult)
                {
                    await _newActionRepository.Delete(id);
                }
                foreach (var Id in actionProposition.IntrestedEmails)
                {
                    await _userNotificationService.GetNotification(userId, NotificationType.ActionStart, activeAction);
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RejectNewAction(string id)
        {
            NewActionProposition actionProposition = await _newActionRepository.Get(id);
            if (actionProposition != null)
            {
                await _newActionRepository.Delete(id);
                return true;
            }
            else
            { 
                return false;
            }
        }
    }
}
