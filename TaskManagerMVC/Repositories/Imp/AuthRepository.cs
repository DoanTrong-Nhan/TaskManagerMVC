using TaskManagerMVC.DBContext;
using TaskManagerMVC.Models;
using TaskManagerMVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TaskManagerMVC.Repositories.Imp
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TaskManagerDbContext _context;

        public AuthRepository(TaskManagerDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetByCredentialsAsync(string username, string password)
        {
            return await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
        }

        public async Task<User?> GetUserAsync(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return null;

            return await GetByIdAsync(userId);
        }

        public async Task<List<string>> GetRolesAsync(User user)
        {
            if (user == null || user.Role == null)
                return new List<string>();

            return new List<string> { user.Role.RoleName };
        }
        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}