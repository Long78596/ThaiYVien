using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Data.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Vui lòng nhập username")]
        public string UserName { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập password")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
