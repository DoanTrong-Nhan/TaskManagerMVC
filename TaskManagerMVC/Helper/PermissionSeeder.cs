using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using TaskManagerMVC.DBContext;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Helper
{
    public class PermissionSeeder
    {
        private readonly IActionDescriptorCollectionProvider _actionProvider;
        private readonly TaskManagerDbContext _context;

        public PermissionSeeder(IActionDescriptorCollectionProvider actionProvider, TaskManagerDbContext context)
        {
            _actionProvider = actionProvider;
            _context = context;
        }

        public void SeedPermissions()
        {
            var existingPermissions = _context.Permissions
                .Select(p => p.Method + ":" + p.Endpoint.ToLower())
                .ToHashSet();

            var newPermissions = new List<Permission>();

            foreach (var actionDescriptor in _actionProvider.ActionDescriptors.Items)
            {
                if (actionDescriptor is ControllerActionDescriptor controllerAction)
                {
                    var method = GetHttpMethod(controllerAction);
                    var endpoint = $"/{controllerAction.ControllerName}/{controllerAction.ActionName}".ToLower();

                    var key = $"{method}:{endpoint}";
                    if (!existingPermissions.Contains(key))
                    {
                        newPermissions.Add(new Permission
                        {
                            PermissionName = $"{controllerAction.ControllerName}_{controllerAction.ActionName}",
                            Method = method,
                            Endpoint = endpoint
                        });
                    }
                }
            }

            if (newPermissions.Any())
            {
                _context.Permissions.AddRange(newPermissions);
                _context.SaveChanges();
            }
        }

        private string GetHttpMethod(ControllerActionDescriptor action)
        {
            var httpMethod = action.MethodInfo
                .GetCustomAttributes(inherit: true)
                .OfType<HttpMethodAttribute>()
                .FirstOrDefault();

            return httpMethod?.HttpMethods.FirstOrDefault() ?? "GET";
        }
    }
}
