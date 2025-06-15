using TaskManagerMVC.DBContext;
using TaskManagerMVC.Models;
using TaskManagerMVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace TaskManagerMVC.Repositories.Imp
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TaskManagerDbContext _context;

        public AuthRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public User? GetByUsername(string username)
        {
            return _context.Users
                           .Include(u => u.Role)
                           .ThenInclude(r => r.RolePermissions)
                           .ThenInclude(rp => rp.Permission)
                           .FirstOrDefault(u => u.Username == username);
        }

        public User? GetById(int userId)
        {
            return _context.Users
                           .Include(u => u.Role)
                           .ThenInclude(r => r.RolePermissions)
                           .ThenInclude(rp => rp.Permission)
                           .FirstOrDefault(u => u.UserId == userId);
        }

        public bool ExistsByUsername(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
