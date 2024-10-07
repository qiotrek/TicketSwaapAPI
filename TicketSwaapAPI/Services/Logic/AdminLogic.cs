using TicketSwaapAPI.Models.Enums;
using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Logic
{

    public interface IAdminLogic
    {
        Task<bool> AcceptNewAction(string id, string userId);
        Task<List<NewActionProposition>> GetActionPropositions();
        Task<List<ProblemsAndQuestionsModel>> GetProblemsAndQuestions();
        Task<bool> OpenProblemsAndQuestions(string userid,string id);
        Task<bool> RejectNewAction(string id);
    }
    public class AdminLogic: IAdminLogic
    {
        private readonly IConfiguration _config;
        private readonly ILogger<NewActionPropositionlogic> _logger;
        private readonly INewActionsPropositionRepository _newActionRepository;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IActiveActionsRepository _activeActionsRepository;
        private readonly IProblemsAndQuestionsRepository _problemsAndQuestionsRepository;

        public AdminLogic(IConfiguration config, ILogger<NewActionPropositionlogic> logger, INewActionsPropositionRepository newActionRepository,/* IAdminRepository adminRepository,*/ IActiveActionsRepository activeActionsRepository, IUserNotificationService userNotificationService,IProblemsAndQuestionsRepository problemsAndQuestionsRepository)
        {
            _config = config;
            _logger = logger;
            _newActionRepository = newActionRepository;
            //_adminRepository = adminRepository;
            _activeActionsRepository = activeActionsRepository;
            _userNotificationService = userNotificationService;
            _problemsAndQuestionsRepository = problemsAndQuestionsRepository;
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

        public async Task<List<ProblemsAndQuestionsModel>> GetProblemsAndQuestions()
        {
            return await _problemsAndQuestionsRepository.GetProblemsAndQuestions();
        }

        public async Task<bool> OpenProblemsAndQuestions(string userId, string id)
        {
            bool result = false;
            ProblemsAndQuestionsModel model = await _problemsAndQuestionsRepository.Get(id);
            model.Status = (int)StatusEnum.Opened;
            model.UpdateDate = DateTime.Now;
            model.UpdateLogin = userId;
            ProblemsAndQuestionsModel resultModel=await _problemsAndQuestionsRepository.Set(model);
            if (resultModel != null)
                result = true;
            return result;
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
