using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Areas.Admin.Dto.Role;
using ThaiYVien.Areas.Admin.Repository.IRepository;

namespace ThaiYVien.Areas.Admin.Repository
{
	public class RoleRepository : IRoleRepository
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public RoleRepository(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}
		public async Task<List<RoleDto>> GetAllRolesAsync()
		{
			return await _roleManager.Roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name })
				 .ToListAsync();
		}
		

		public async Task<RoleDto> GetRoleByIdAsync(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			return role == null ? null : new RoleDto { Id = role.Id, Name = role.Name };
		}

		public async Task<bool> CreateRoleAsync(RoleDto dto)
		{
			if (await _roleManager.RoleExistsAsync(dto.Name))
				return false;

			var result = await _roleManager.CreateAsync(new IdentityRole(dto.Name));
			return result.Succeeded;
		}

		public async Task<bool> UpdateRoleAsync(string id, RoleDto dto)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
				return false;

			role.Name = dto.Name;
			var result = await _roleManager.UpdateAsync(role);
			return result.Succeeded;
		}

		public async Task<bool> DeleteRoleAsync(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
				return false;

			var result = await _roleManager.DeleteAsync(role);
			return result.Succeeded;
		}
	
   }
}
