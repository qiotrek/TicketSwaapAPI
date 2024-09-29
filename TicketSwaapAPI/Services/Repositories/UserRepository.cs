using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using TicketSwaapAPI.Services.Infrastructure;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.Services.Repositories
{
    public interface IUserRepository
    {
        public Task<UserModel> Get(string Id);
        public Task<UserModel> Set(UserModel user);
        public Task<UserModel> Update(UserModel user);
        public Task<UserModel> Create(UserModel user);
        public Task<UserModel> Delete(UserModel user);
        Task<List<UserModel>> GetUsers();
    }

    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserRepository> _logger;


        private readonly FireStoreService _fsSvc;
        private readonly FirestoreTableNamesConfig _fsTabs;


        public UserRepository(IConfiguration config, ILogger<UserRepository> logger, FireStoreService fsSvc, FirestoreTableNamesConfig fsTabs)
        {
            _logger = logger;
            _config = config;
            _fsSvc = fsSvc;
            _fsTabs = fsTabs;
        }
        public async Task<UserModel> Get(string Id)
        {
            return await _fsSvc.Get<UserModel>(_fsTabs.UsersTableName, Id);
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await _fsSvc.GetList<UserModel>(_fsTabs.UsersTableName, null);
        }

        public async Task<UserModel> Set(UserModel user)
        {

            var result = await _fsSvc.Set(_fsTabs.UsersTableName, user.Id, user);

            return user;

        }

        public async Task<UserModel> Update(UserModel user)
        {
            var result = await _fsSvc.Set(_fsTabs.UsersTableName, user.Id, user);
            return user;
        }
        public async Task<UserModel> Create(UserModel user)
        {
            var result = await _fsSvc.Set(_fsTabs.UsersTableName, user.Id, user);
            return user;

        }
        public async Task<UserModel> Delete(UserModel user)
        {
            var result = await _fsSvc.Set(_fsTabs.DeletedUsersTableName, user.Id, user);
            await _fsSvc.Delete(_fsTabs.UsersTableName, user.Id);
            return user;
        }


    }
}
