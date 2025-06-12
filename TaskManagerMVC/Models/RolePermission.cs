namespace TaskManagerMVC.Models
{
    public class RolePermission
    {
        private int _roleId;
        private Role _role = null!;

        private int _permissionId;
        private Permission _permission = null!;

        public int RoleId
        {
            get => _roleId;
            private set => _roleId = value;
        }

        public Role Role
        {
            get => _role;
            set => _role = value ?? throw new ArgumentNullException(nameof(Role));
        }

        public int PermissionId
        {
            get => _permissionId;
            private set => _permissionId = value;
        }

        public Permission Permission
        {
            get => _permission;
            set => _permission = value ?? throw new ArgumentNullException(nameof(Permission));
        }

        public RolePermission(int roleId, int permissionId, Role role, Permission permission)
        {
            _roleId = roleId;
            _permissionId = permissionId;
            Role = role;
            Permission = permission;
        }

        private RolePermission() { }
    }
}
