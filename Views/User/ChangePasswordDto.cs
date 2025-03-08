using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Data.Dto.User
{
    public class ChangePasswordDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$",
            ErrorMessage = "Mật khẩu mới phải có ít nhất 1 chữ và 1 số")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmNewPassword { get; set; }
    }
}
