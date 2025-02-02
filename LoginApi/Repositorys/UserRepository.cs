using LoginApi.Entities;
using LoginApi.Repositorys.Contracts;
using MongoDB.Driver;

namespace LoginApi.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task AddUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
           
        }

        public async Task DeleteUserAsync(string username)
        {
            await _users.DeleteOneAsync(u => u.UserName == username);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _users.Find(_=> true).ToListAsync();
        }

        public async Task<User> GetUsernameAsync(string username)
        {
           return await _users.Find(u=> u.UserName == username).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersByStore(string storeCode)
        {
            return await _users.Find(u => u.StoreCode == storeCode).ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.UserName == user.UserName, user);
        }
    }
}
