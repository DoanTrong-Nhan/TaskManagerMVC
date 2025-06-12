using System;
using System.Collections.Generic;

namespace TaskManagerMVC.Models
{
    public class User
    {
        private string _username = null!;
        private string _passwordHash = null!;
        private string? _fullName;
        private string? _email;
        private int? _roleId;
        private Role? _role;
        private ICollection<Task> _tasks = new List<Task>();

        public int UserId { get; private set; }

        public string Username
        {
            get => _username;
            set => _username = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Username cannot be null or empty.");
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("PasswordHash cannot be null or empty.");
        }

        public string? FullName
        {
            get => _fullName;
            set => _fullName = value;
        }

        public string? Email
        {
            get => _email;
            set => _email = value;
        }

        public int? RoleId
        {
            get => _roleId;
            private set => _roleId = value;
        }

        public Role? Role
        {
            get => _role;
            set => _role = value;
        }

        public virtual ICollection<Task> Tasks
        {
            get => _tasks;
            private set => _tasks = value;
        }

        private User() { }

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
    }
}
