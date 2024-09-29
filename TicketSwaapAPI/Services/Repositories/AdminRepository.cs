using TicketSwaapAPI.Services.Infrastructure;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Repositories
{
    public interface IAdminRepository
    { 
    
    }
    public class AdminRepository: IAdminRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AdminRepository> _logger;
        private readonly FireStoreService _fsSvc;
        private readonly FirestoreTableNamesConfig _fsTabs;

        public AdminRepository(IConfiguration config, ILogger<AdminRepository> logger, FireStoreService fsSvc, FirestoreTableNamesConfig fsTabs)
        {
            _config = config;
            _logger = logger;
            _fsSvc = fsSvc;
            _fsTabs = fsTabs;
        }


    }
}
