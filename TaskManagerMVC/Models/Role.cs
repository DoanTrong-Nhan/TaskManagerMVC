using System.Collections.Generic;

namespace TaskManagerMVC.Models
{
    public class Role
    {
        private string _roleName = null!;
        private ICollection<User> _users = new List<User>();
        private ICollection<Permission> _permissions = new List<Permission>();
        private ICollection<RolePermission> _rolePermissions = new List<RolePermission>();

        public int RoleId { get; private set; }

        public string RoleName
        {
            get => _roleName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _roleName = value;
            }
        }

        public virtual ICollection<User> Users
        {
            get => _users;
            private set => _users = value;
        }

        public virtual ICollection<Permission> Permissions
        {
            get => _permissions;
            private set => _permissions = value;
        }

        public virtual ICollection<RolePermission> RolePermissions
        {
            get => _rolePermissions;
            private set => _rolePermissions = value;
        }
    }
}
