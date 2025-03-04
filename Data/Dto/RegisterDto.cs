using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Data.Dto
{

    public class RegisterDto
    {
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự.")]
        [MinLength(3, ErrorMessage = "Tên tài khoản phải có ít nhất 3  ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }
    }

}
