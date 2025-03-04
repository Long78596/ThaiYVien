using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Data.Dto
{
    public class ForgetPassDto
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

    }
}
