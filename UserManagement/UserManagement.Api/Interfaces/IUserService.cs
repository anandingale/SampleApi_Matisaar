using UserManagement.Domain;

namespace UserManagement.Api.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User> GetUserById(int id);
        public Task<User> GetUserByName(string name);
        public Task<User> AddUser(User user);

        public Task<User> UpdateUserById(User user);

        public Task<bool> RemoveUserById(int id);
        
    }
}
