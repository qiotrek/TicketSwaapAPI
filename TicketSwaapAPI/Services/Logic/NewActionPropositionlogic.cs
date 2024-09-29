using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Logic
{
    public interface INewActionPropositionlogic
    {
        Task<List<NewActionProposition>> GetList();
        Task<NewActionProposition> AddNewActionRequest(string id, string name, string img, DateTime date, string user);
        Task<bool> CheckProposition(string id, string user);
    }
    public class NewActionPropositionlogic:INewActionPropositionlogic
    {

        private readonly IConfiguration _config;
        private readonly ILogger<NewActionPropositionlogic> _logger;
        private readonly INewActionsPropositionRepository _repository;
        public NewActionPropositionlogic(IConfiguration configuration, ILogger<NewActionPropositionlogic> logger, INewActionsPropositionRepository repository) 
        {
            _config = configuration;
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> CheckProposition(string id,string user)
        {
            var propositon= await _repository.Get(id);
            if (propositon != null && propositon.Id == id)
            {
                return propositon.IntrestedEmails.Contains(user);
            }
            return false;
        }
        public async Task<List<NewActionProposition>> GetList()
        {
            return await _repository.GetActionPropositions();
        }

        public async Task<NewActionProposition> AddNewActionRequest(string id,string name,string img , DateTime date,string user)
        {
            NewActionProposition isExist = await _repository.Get(id);
            if (isExist!=null&&isExist.Id == id)
            {
                if (!isExist.IntrestedEmails.Contains(user))
                {
                    isExist.IntrestedCount++;
                    isExist.IntrestedEmails.Add(user);
                    await _repository.Set(isExist);
                }
                return isExist;
            }
            else
            {
                NewActionProposition newActionProposition= new NewActionProposition();
                newActionProposition.Id = id;
                newActionProposition.Name = name;
                newActionProposition.EventDate = date;
                newActionProposition.Img = img;
                newActionProposition.IntrestedCount = 1;
                newActionProposition.IntrestedEmails = new List<string>();
                newActionProposition.IntrestedEmails.Add(user);

                await _repository.Set(newActionProposition);
                return newActionProposition;
            }
            
        }
    }
}
