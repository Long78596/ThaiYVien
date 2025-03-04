using ThaiYVien.Models;

namespace ThaiYVien.Areas.Admin.Dto.User
{
    public class UserWithRoleDto
    {
        public AppUserModel User { get; set; }
        public string? RoleName
        {
            get; set;
        }
    }
}
