using TaskManagerMVC.Dto.Auth;
using TaskManagerMVC.Models;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Services.Imp
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<User?> ValidateUserAsync(LoginDto loginDto)
        {
            return await _authRepository.GetByCredentialsAsync(loginDto.Username, loginDto.Password);
        }

    }
}
