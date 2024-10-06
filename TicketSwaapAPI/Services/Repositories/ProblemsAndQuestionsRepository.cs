using TicketSwaapAPI.Services.Infrastructure;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Repositories
{
    public interface IProblemsAndQuestionsRepository
    {
        Task<bool> Delete(ProblemsAndQuestionsModel QaP);
        Task<ProblemsAndQuestionsModel> Get(string Id);
        Task<List<ProblemsAndQuestionsModel>> GetProblemsAndQuestions();
        Task<ProblemsAndQuestionsModel> Set(ProblemsAndQuestionsModel QaP);
    }

    public class ProblemsAndQuestionsRepository : IProblemsAndQuestionsRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ProblemsAndQuestionsRepository> _logger;


        private readonly FireStoreService _fsSvc;
        private readonly FirestoreTableNamesConfig _fsTabs;


        public ProblemsAndQuestionsRepository(IConfiguration config, ILogger<ProblemsAndQuestionsRepository> logger, FireStoreService fsSvc, FirestoreTableNamesConfig fsTabs)
        {
            _logger = logger;
            _config = config;
            _fsSvc = fsSvc;
            _fsTabs = fsTabs;
        }
        public async Task<ProblemsAndQuestionsModel> Get(string Id)
        {
            return await _fsSvc.Get<ProblemsAndQuestionsModel>(_fsTabs.ProblemsAndQuestionsTableName, Id);
        }

        public async Task<List<ProblemsAndQuestionsModel>> GetProblemsAndQuestions()
        {
            return await _fsSvc.GetList<ProblemsAndQuestionsModel>(_fsTabs.ProblemsAndQuestionsTableName, null);
        }

        public async Task<ProblemsAndQuestionsModel> Set(ProblemsAndQuestionsModel QaP)
        {

            var result = await _fsSvc.Set(_fsTabs.ProblemsAndQuestionsTableName, QaP.Id, QaP);

            return QaP;

        }

        public async Task<bool> Delete(ProblemsAndQuestionsModel QaP)
        {
            var result = await _fsSvc.Delete(_fsTabs.ProblemsAndQuestionsTableName, QaP.Id);
            return true;
        }

    }
}
