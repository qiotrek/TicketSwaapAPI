using TicketSwaapAPI.Services.Infrastructure;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Repositories
{
    public interface IActiveActionsRepository
    {
        Task<bool> Delete(ActiveActionModel action);
        Task<ActiveActionModel> Get(string Id);
        Task<List<ActiveActionModel>> GetList();
        Task<bool> Set(ActiveActionModel action);
    }

    public class ActiveActionsRepository : IActiveActionsRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ActiveActionsRepository> _logger;


        private readonly FireStoreService _fsSvc;
        private readonly FirestoreTableNamesConfig _fsTabs;
        public ActiveActionsRepository(IConfiguration configuration, ILogger<ActiveActionsRepository> logger, FireStoreService fireStoreService, FirestoreTableNamesConfig firestoreTableNamesConfig)
        {
            _config = configuration;
            _logger = logger;
            _fsSvc = fireStoreService;
            _fsTabs = firestoreTableNamesConfig;
        }

        public async Task<ActiveActionModel> Get(string Id)
        {
            return await _fsSvc.Get<ActiveActionModel>(_fsTabs.ActiveActionsTableName, Id);
        }

        public async Task<List<ActiveActionModel>> GetList()
        {
            return await _fsSvc.GetList<ActiveActionModel>(_fsTabs.ActiveActionsTableName, null);
        }

        public async Task<bool> Set(ActiveActionModel action)
        {

            var result = await _fsSvc.Set(_fsTabs.ActiveActionsTableName, action.Id, action);

            return true;

        }
        public async Task<bool> Delete(ActiveActionModel action)
        {
            await _fsSvc.Delete(_fsTabs.ActiveActionsTableName, action.Id);
            return true;
        }
    }
}
