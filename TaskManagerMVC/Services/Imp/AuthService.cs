﻿using Microsoft.AspNetCore.Identity;
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

            return permissions.Any(p =>
                  p.Method.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                  endpoint.StartsWith(p.Endpoint, StringComparison.OrdinalIgnoreCase));

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

        public int GetCurrentUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        public async Task<int> GetUserRoleIdAsync(ClaimsPrincipal user)
        {
            var appUser = await _authRepository.GetUserAsync(user);
            if (appUser == null || appUser.Role == null)
                return 0;

            return appUser.Role.RoleId; // Lấy trực tiếp RoleId từ user.Role
        }
        // admin phan quyen
        public async Task<RolePermissionDto> GetRolePermissionAsync(int roleId)
        {
            var role = await _authRepository.GetRoleWithPermissionsAsync(roleId);
            var allPermissions = await _authRepository.GetAllPermissionsAsync();

            if (role == null)
                return new RolePermissionDto();

            var dto = new RolePermissionDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                PermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList(),
                AvailablePermissions = allPermissions.Select(p => new PermissionDto
                {
                    PermissionId = p.PermissionId,
                    PermissionName = p.PermissionName,
                    Method = p.Method,
                    Endpoint = p.Endpoint,
                    IsAssigned = role.RolePermissions.Any(rp => rp.PermissionId == p.PermissionId)
                }).ToList()
            };

            return dto;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _authRepository.GetAllRolesAsync();
        }

        public async System.Threading.Tasks.Task UpdateRolePermissionsAsync(int roleId, List<int> permissionIds)
        {
            await _authRepository.UpdateRolePermissionsAsync(roleId, permissionIds);
        }



    }
}
