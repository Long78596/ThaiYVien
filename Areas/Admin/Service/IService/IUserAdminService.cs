
using ThaiYVien.Areas.Admin.Dto.User;

namespace ThaiYVien.Areas.Admin.Service.IService
{
	public interface IUserAdminService
	{

		Task<List<UserResponseDto>> GetAllUsersAsync();
		Task<UserResponseDto?> GetUserByIdAsync(string id);
		Task<string> CreateUserAsync(UserCreateDto userDto);
		Task<bool> DeleteUserAsync(string id);
		Task<bool> UpdateUserAsync(UserUpdateDto userDto);
	}
}
