using TaskManagerMVC.Models;

namespace TaskManagerMVC.Services.Interfaces
{
    public interface IAuthService
    {
        string Login(string username, string password);
        void Register(User newUser);
    }

}
