using LoginApi.Entities;

namespace LoginApi.Repositorys.Contracts
{
    public interface IUserRepository
    {
        Task <User> GetUsernameAsync(string username);
        Task AddUserAsync(User user);   
        
        Task UpdateUserAsync(User user);

        Task <List<User>>GetAllUsersAsync();

        Task DeleteUserAsync(string username);

        Task<List<User>> GetUsersByStore(string storeCode);
    }
}
