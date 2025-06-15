namespace TaskManagerMVC.JWT
{
    // Lớp cấu hình chứa các thông số liên quan đến JWT
    public class JwtSetting
    {
        // Khóa bí mật để mã hóa JWT (dùng để ký token)
        public string? Secret { get; set; }

        // Số giờ token tồn tại kể từ thời điểm phát hành
        public int ExpiryHours { get; set; }

        // Đơn vị phát hành token (issuer)
        public string? Issuer { get; set; }

        // Đối tượng nhận token (audience)
        public string? Audience { get; set; }
    }
}
