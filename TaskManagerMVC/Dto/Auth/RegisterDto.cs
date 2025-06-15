using System.ComponentModel.DataAnnotations;

namespace TaskManagerMVC.Dto.Auth
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        public int RoleId { get; set; }
    }
}