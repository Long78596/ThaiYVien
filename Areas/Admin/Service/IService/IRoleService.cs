using ThaiYVien.Areas.Admin.Dto.Role;

namespace ThaiYVien.Areas.Admin.Service.IService
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
		Task<RoleDto> GetRoleByIdAsync(string id);
		Task<bool> CreateRoleAsync(RoleDto dto);
		Task<bool> UpdateRoleAsync(string id, RoleDto dto);
		Task<bool> DeleteRoleAsync(string id);
	}
}
