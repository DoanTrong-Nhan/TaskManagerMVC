using TaskManagerMVC.Models;

namespace TaskManagerMVC.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        User? GetByUsername(string username);
        User? GetById(int userId);
        bool ExistsByUsername(string username);
        void Add(User user);
    }
}
