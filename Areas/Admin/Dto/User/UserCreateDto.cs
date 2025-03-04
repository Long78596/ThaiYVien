using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Areas.Admin.Dto.User
{
	public class UserCreateDto
	{

		[Required(ErrorMessage = "Tên tài khoản là bắt buộc.")]
		[StringLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự.")]
		[MinLength(3, ErrorMessage = "Tên tài khoản phải có ít nhất 3 ký tự.")]
		public string UserName { get; set; }
        [Required(ErrorMessage = "Tên  là bắt buộc phải nhập.")]
        [StringLength(50, ErrorMessage = "Tên  không được vượt quá 50 ký tự.")]
        [MinLength(3, ErrorMessage = "Tên  phải có ít nhất 3 ký tự.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ.")]
		[StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
		[RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng số 0 và có từ 10 đến 11 chữ số.")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Vai trò là bắt buộc.")]
		public string RoleId { get; set; }

		[Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
		public string PasswordHash { get; set; }
	}
}
