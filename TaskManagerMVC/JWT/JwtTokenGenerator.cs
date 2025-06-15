using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.JWT
{
    // Lớp chịu trách nhiệm tạo JWT token cho người dùng
    public class JwtTokenGenerator
    {
        private readonly JwtSetting _jwtSettings;

        // Constructor inject cấu hình JWT
        public JwtTokenGenerator(JwtSetting jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        // Trả về thời gian  của token
        public int ExpiryHours => _jwtSettings.ExpiryHours;

        // Hàm tạo JWT token từ đối tượng người dùng
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler(); // Dùng để tạo và xử lý token
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret!); // Chuyển khóa bí mật thành mảng byte

            if (string.IsNullOrEmpty(_jwtSettings.Secret))
            {
                throw new InvalidOperationException("JWT Secret is not configured.");
            }

            // Tạo danh sách claim gắn với người dùng
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Lấy từ property trực tiếp
                   new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role?.RoleName ?? string.Empty)
                };


            // Cấu hình cho token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Gán danh tính chứa claims
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours), // Thời hạn hết hạn
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ), // Ký token bằng thuật toán HMAC SHA256
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience

            };

            // Tạo token từ mô tả
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Trả token dạng chuỗi
            return tokenHandler.WriteToken(token);
        }
    }
}
