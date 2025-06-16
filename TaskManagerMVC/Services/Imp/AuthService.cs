using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using TaskManagerMVC.Dto.Auth;
using TaskManagerMVC.Models;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Services.Imp
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMemoryCache _cache;

        public AuthService(IAuthRepository authRepository, IMemoryCache cache)
        {
            _authRepository = authRepository;
            _cache = cache;
        }

        public async Task<User?> ValidateUserAsync(LoginDto loginDto)
        {
            return await _authRepository.GetByCredentialsAsync(loginDto.Username, loginDto.Password);
        }

        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string method, string endpoint)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return false;

            var permissions = await GetPermissionsForUserAsync(userId);
            var key = $"{method}:{endpoint}".ToLower();

            return permissions.Any(p => key.Contains($"{p.Method}:{p.Endpoint}".ToLower()));
        }

        private async Task<List<(string Method, string Endpoint)>> GetPermissionsForUserAsync(int userId)
        {
            var cacheKey = $"permissions_user_{userId}";

            if (!_cache.TryGetValue(cacheKey, out List<(string Method, string Endpoint)> permissions))
            {
                var user = await _authRepository.GetByIdAsync(userId);
                permissions = user?.Role?.RolePermissions?
                    .Select(rp => (rp.Permission.Method, rp.Permission.Endpoint))
                    .ToList() ?? new List<(string, string)>();

                // Cache for 10 minutes
                _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(10));
            }

            return permissions;
        }

    }
}
