

using ThaiYVien.Areas.Admin.Dto.User;
using ThaiYVien.Models;

namespace ThaiYVien.Areas.Admin.Repository.IRepository
{
	public interface IUserAdminRepository
	{

		Task<List<UserResponseDto>> GetAllUsersAsync();
		Task<UserResponseDto?> GetUserByIdAsync(string id);
		Task<bool> CheckUsernameExists(string username);
		Task<bool> CheckEmailExists(string email);
		Task<AppUserModel> CreateUserAsync(AppUserModel user, string password);
		Task<AppUserModel> GetUserByEmailAsync(string email);
		Task<bool> DeleteUserAsync(string id);
		Task<bool> UpdateUserAsync(AppUserModel user, string roleId);
	}
}
