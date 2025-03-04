using ThaiYVien.Areas.Admin.Dto.Role;
using ThaiYVien.Areas.Admin.Repository.IRepository;
using ThaiYVien.Areas.Admin.Service.IService;

namespace ThaiYVien.Areas.Admin.Service
{
	public class RoleService : IRoleService
	{

		private readonly IRoleRepository _roleRepository;
		public RoleService(IRoleRepository roleRepository)
		{
			_roleRepository = roleRepository;
		}

		public async Task<List<RoleDto>> GetAllRolesAsync()
		{
			return await _roleRepository.GetAllRolesAsync();
		}
		public async Task<RoleDto> GetRoleByIdAsync(string id)
		{
			return await _roleRepository.GetRoleByIdAsync(id);
		}

		public async Task<bool> CreateRoleAsync(RoleDto dto)
		{
			return await _roleRepository.CreateRoleAsync(dto);
		}

		public async Task<bool> UpdateRoleAsync(string id, RoleDto dto)
		{
			return await _roleRepository.UpdateRoleAsync(id, dto);
		}

		public async Task<bool> DeleteRoleAsync(string id)
		{
			return await _roleRepository.DeleteRoleAsync(id);
		}
	}
}
