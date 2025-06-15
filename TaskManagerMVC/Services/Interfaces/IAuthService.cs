using TaskManagerMVC.Dto.Auth;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(LoginDto loginDto);
    }
}
