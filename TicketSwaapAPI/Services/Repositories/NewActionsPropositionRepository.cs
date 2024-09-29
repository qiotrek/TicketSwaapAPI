using TicketSwaapAPI.Services.Infrastructure;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Repositories
{
    public interface INewActionsPropositionRepository
    {
        public Task<NewActionProposition> Get(string Id);
        public Task<List<NewActionProposition>> GetActionPropositions();
        public Task<NewActionProposition> Set(NewActionProposition newActionPropos);
        public Task<bool> Delete(string id);
    }
    public class NewActionsPropositionRepository: INewActionsPropositionRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<NewActionsPropositionRepository> _logger;


        private readonly FireStoreService _fsSvc;
        private readonly FirestoreTableNamesConfig _fsTabs;
        public NewActionsPropositionRepository(IConfiguration configuration, ILogger<NewActionsPropositionRepository> logger, FireStoreService fireStoreService, FirestoreTableNamesConfig firestoreTableNamesConfig ) 
        { 
            _config = configuration;
            _logger = logger;
            _fsSvc = fireStoreService;
            _fsTabs = firestoreTableNamesConfig;
        }

        public async Task<NewActionProposition> Get(string Id)
        {
            return await _fsSvc.Get<NewActionProposition>(_fsTabs.NewActionsPropositionsTableName, Id);
        }

        public async Task<List<NewActionProposition>> GetActionPropositions()
        {
            return await _fsSvc.GetList<NewActionProposition>(_fsTabs.NewActionsPropositionsTableName, null);
        }

        public async Task<NewActionProposition> Set(NewActionProposition newActionPropos)
        {

            var result = await _fsSvc.Set(_fsTabs.NewActionsPropositionsTableName, newActionPropos.Id, newActionPropos);

            return newActionPropos;

        }
        public async Task<bool> Delete(string id)
        {
            await _fsSvc.Delete(_fsTabs.NewActionsPropositionsTableName, id);
            return true;
        }
    }
}
