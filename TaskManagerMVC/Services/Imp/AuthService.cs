using System.Text;
using TaskManagerMVC.JWT;
using TaskManagerMVC.Models;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Services.Imp
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IAuthRepository authRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _authRepository = authRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public string Login(string username, string password)
        {
            var user = _authRepository.GetByUsername(username);

            if (user == null || user.PasswordHash != HashPassword(password))
                throw new UnauthorizedAccessException("Invalid credentials");

            return _jwtTokenGenerator.GenerateJwtToken(user);
        }

        public void Register(User newUser)
        {
            if (_authRepository.ExistsByUsername(newUser.Username))
                throw new Exception("Username already exists");

            newUser.PasswordHash = HashPassword(newUser.PasswordHash);
            _authRepository.Add(newUser);
        }

        private string HashPassword(string password)
        {
            // Hash đơn giản, có thể thay bằng BCrypt/SHA256
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }
    }

}
