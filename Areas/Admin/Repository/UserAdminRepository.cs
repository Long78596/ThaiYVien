

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Areas.Admin.Dto.User;
using ThaiYVien.Areas.Admin.Repository.IRepository;
using ThaiYVien.Data;
using ThaiYVien.Models;

namespace ThaiYVien.Areas.Admin.Repository
{
	public class UserAdminRepository:IUserAdminRepository
	{
		private readonly UserManager<AppUserModel> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;


		public UserAdminRepository(UserManager<AppUserModel> userManager, ApplicationDbContext context, IMapper mapper)
		{
			_userManager = userManager;
			_context = context;
			_mapper = mapper;
		}


		public async Task<List<UserResponseDto>> GetAllUsersAsync()
		{
			var users = await _userManager.Users.ToListAsync();
			var userDtos = _mapper.Map<List<UserResponseDto>>(users);

			foreach (var userDto in userDtos)
			{
				var user = await _userManager.FindByIdAsync(userDto.Id);
				var roles = await _userManager.GetRolesAsync(user);
				userDto.RoleName = roles.FirstOrDefault() ?? "No Role";
			}

			return userDtos;
		}

		public async Task<UserResponseDto?> GetUserByIdAsync(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return null;

			var userDto = _mapper.Map<UserResponseDto>(user);
			var roles = await _userManager.GetRolesAsync(user);
			userDto.RoleName = roles.FirstOrDefault() ?? "No Role";

			return userDto;
		}
		public async Task<bool> CheckUsernameExists(string username)
		{
			return await _userManager.Users.AnyAsync(u => u.UserName == username);
		}

		public async Task<bool> CheckEmailExists(string email)
		{
			return await _userManager.Users.AnyAsync(u => u.Email == email);
		}

		public async Task<AppUserModel> CreateUserAsync(AppUserModel user, string password)
		{
			var result = await _userManager.CreateAsync(user, password);
			return result.Succeeded ? user : null;
		}

		public async Task<AppUserModel> GetUserByEmailAsync(string email)
		{
			return await _userManager.FindByEmailAsync(email);
		}

		

		public async Task<bool> DeleteUserAsync(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return false;

			var result = await _userManager.DeleteAsync(user);
			return result.Succeeded;
		}

		public async Task<bool> UpdateUserAsync(AppUserModel user, string roleId)
		{
			var existingUser = await _userManager.FindByIdAsync(user.Id);
			if (existingUser == null) return false;

			_mapper.Map(user, existingUser);
			var result = await _userManager.UpdateAsync(existingUser);
			return result.Succeeded;
		}
	}
}

