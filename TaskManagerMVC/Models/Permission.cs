using System.Collections.Generic;

namespace TaskManagerMVC.Models
{
    public class Permission
    {
        private string _permissionName = null!;
        private string _method = null!;
        private string _endpoint = null!;
        private ICollection<Role> _roles = new List<Role>();
        private ICollection<RolePermission> _rolePermissions = new List<RolePermission>();

        public int PermissionId { get; private set; }

        public string PermissionName
        {
            get => _permissionName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _permissionName = value;
            }
        }

        public string Method
        {
            get => _method;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _method = value;
            }
        }

        public string Endpoint
        {
            get => _endpoint;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _endpoint = value;
            }
        }

        public virtual ICollection<Role> Roles
        {
            get => _roles;
            private set => _roles = value;
        }

        public virtual ICollection<RolePermission> RolePermissions
        {
            get => _rolePermissions;
            private set => _rolePermissions = value;
        }
    }
}
