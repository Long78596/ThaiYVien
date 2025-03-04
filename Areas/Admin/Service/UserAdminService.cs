using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto;
using ThaiYVien.Areas.Admin.Dto.User;
using ThaiYVien.Areas.Admin.Repository.IRepository;
using ThaiYVien.Areas.Admin.Service.IService;
using ThaiYVien.Models;

namespace ThaiYVien.Areas.Admin.Service
{
	public class UserAdminService:IUserAdminService
	{
		private readonly IUserAdminRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<AppUserModel> _userManager;

		public UserAdminService(IUserAdminRepository userRepository, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<AppUserModel> userManager)
		{
			_userRepository = userRepository;
			_mapper = mapper;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task<List<UserResponseDto>> GetAllUsersAsync()
		{
			return await _userRepository.GetAllUsersAsync();
		}

		public async Task<UserResponseDto?> GetUserByIdAsync(string id)
		{
			return await _userRepository.GetUserByIdAsync(id);
		}



		public async Task<string> CreateUserAsync(UserCreateDto userDto)
		{
			if (string.IsNullOrWhiteSpace(userDto.PasswordHash) ||
				userDto.PasswordHash.Length < 6 ||
				!userDto.PasswordHash.Any(char.IsLetter) ||
				!userDto.PasswordHash.Any(char.IsDigit))
			{
				return "Mật khẩu phải có ít nhất 6 ký tự, gồm cả chữ và số!";
			}

			if (await _userRepository.CheckUsernameExists(userDto.UserName))
			{
				return "Tên tài khoản đã tồn tại";
			}

			if (await _userRepository.CheckEmailExists(userDto.Email))
			{
				return "Email đã tồn tại";
			}

			var newUser = _mapper.Map<AppUserModel>(userDto);
			var createdUser = await _userRepository.CreateUserAsync(newUser, userDto.PasswordHash);

			if (createdUser == null)
			{
				return "Lỗi khi tạo tài khoản";
			}

			var role = await _roleManager.FindByIdAsync(userDto.RoleId);
			if (role == null)
			{
				return "Không tìm thấy quyền!";
			}

			await _userManager.AddToRoleAsync(createdUser, role.Name);
			return "Thêm người dùng thành công!";
		}
	




	public async Task<bool> DeleteUserAsync(string id)
		{
			return await _userRepository.DeleteUserAsync(id);
		}

		public async Task<bool> UpdateUserAsync(UserUpdateDto userDto)
		{
			var user = _mapper.Map<AppUserModel>(userDto);
			return await _userRepository.UpdateUserAsync(user, userDto.RoleId);
		}

		
	}
}
