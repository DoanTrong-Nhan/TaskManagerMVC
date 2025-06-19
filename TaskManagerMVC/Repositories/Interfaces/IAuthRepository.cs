using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByCredentialsAsync(string username, string password);

        Task<User?> GetByIdAsync(int userId);

        Task<User?> GetUserAsync(ClaimsPrincipal user); // Thêm phương thức này
        Task<List<string>> GetRolesAsync(User user); // Thêm phương thức này
        Task<Role?> GetRoleByNameAsync(string roleName);

        // Phân quyền
        Task<List<Permission>> GetAllPermissionsAsync();

        Task<List<Role>> GetAllRolesAsync();

        Task<Role?> GetRoleWithPermissionsAsync(int roleId);

        System.Threading.Tasks.Task UpdateRolePermissionsAsync(int roleId, List<int> permissionIds);



    }
}
