using TicketSwaapAPI.Services.Infrastructure;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Repositories
{
    public interface IOffertRepository
    {
        Task<bool> Delete(OffertModel offert);
        Task<OffertModel> Get(string Id);
        Task<List<OffertModel>> GetList();
        Task<bool> Set(OffertModel offert);
    }

    public class OffertRepository : IOffertRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<OffertRepository> _logger;


        private readonly FireStoreService _fsSvc;
        private readonly FirestoreTableNamesConfig _fsTabs;
        public OffertRepository(IConfiguration configuration, ILogger<OffertRepository> logger, FireStoreService fireStoreService, FirestoreTableNamesConfig firestoreTableNamesConfig)
        {
            _config = configuration;
            _logger = logger;
            _fsSvc = fireStoreService;
            _fsTabs = firestoreTableNamesConfig;
        }

        public async Task<OffertModel> Get(string Id)
        {
            return await _fsSvc.Get<OffertModel>(_fsTabs.OffertsTableName, Id);
        }

        public async Task<List<OffertModel>> GetList()
        {
            return await _fsSvc.GetList<OffertModel>(_fsTabs.OffertsTableName, null);
        }

        public async Task<bool> Set(OffertModel offert)
        {

            var result = await _fsSvc.Set(_fsTabs.OffertsTableName, offert.Id, offert);

            return true;

        }
        public async Task<bool> Delete(OffertModel offert)
        {
            await _fsSvc.Delete(_fsTabs.OffertsTableName, offert.Id);
            return true;
        }

    }
}
