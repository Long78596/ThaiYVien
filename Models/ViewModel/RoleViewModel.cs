using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ThaiYVien.Areas.Admin.Dto.Role;

namespace ThaiYVien.Models.ViewModel
{
    public class RoleViewModel
    {
		public List<RoleDto> Roles { get; set; } 
		public RoleDto Role { get; set; } 
		public string Name { get; set; }
		public string Id { get; set; }
	}
}
