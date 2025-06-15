using TaskManagerMVC.Models;

namespace TaskManagerMVC.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByCredentialsAsync(string username, string password);

        Task<User?> GetByIdAsync(int userId);
    }
}
