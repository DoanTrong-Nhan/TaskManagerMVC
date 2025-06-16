using System.Security.Claims;
using TaskManagerMVC.Dto.Auth;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(LoginDto loginDto);

        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string method, string endpoint);
    }
}
