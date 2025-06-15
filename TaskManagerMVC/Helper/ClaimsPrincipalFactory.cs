using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using TaskManagerMVC.Dto.Auth;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Helpers
{
    public static class ClaimsPrincipalFactory
    {
        public static ClaimsPrincipal CreatePrincipal(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            if (!string.IsNullOrEmpty(user.Role?.RoleName))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.RoleName));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
    }
}
