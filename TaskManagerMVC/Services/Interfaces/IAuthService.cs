using System.Security.Claims;
using TaskManagerMVC.Dto.Auth;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(LoginDto loginDto);

        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string method, string endpoint);

        // Authorziration
        int GetCurrentUserId(ClaimsPrincipal user);
        Task<int> GetUserRoleIdAsync(ClaimsPrincipal user);

        //ad min phan quyen 

        Task<RolePermissionDto> GetRolePermissionAsync(int roleId);

        System.Threading.Tasks.Task UpdateRolePermissionsAsync(int roleId, List<int> permissionIds);
        Task<List<Role>> GetAllRolesAsync();
    }
}
