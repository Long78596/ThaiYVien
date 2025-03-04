using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Areas.Admin.Dto.Role
{
	public class RoleDto
	{
		public string Id { get; set; }

		[Required(ErrorMessage = "Tên quyền không được để trống")]
		public string Name { get; set; }
	}
}
