using System.Collections.Generic;

namespace TaskManagerMVC.Dto.Auth
{
    public class RolePermissionDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<int> PermissionIds { get; set; } = new List<int>();
        public List<PermissionDto> AvailablePermissions { get; set; } = new List<PermissionDto>();
    }

    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public bool IsAssigned { get; set; }
    }
}
