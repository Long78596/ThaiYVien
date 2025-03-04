using Microsoft.AspNetCore.Identity;
using ThaiYVien.Data.Dto;
using ThaiYVien.Models;
using ThaiYVien.Repository.IRepository;
using ThaiYVien.Response;
using ThaiYVien.Services.IService;

namespace ThaiYVien.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<AppUserModel> _signInManager;

        public AccountService(IUserRepository userRepository, SignInManager<AppUserModel> signInManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.FindByUsernameOrEmailAsync(loginDto.UserName);
            if (user == null)
            {
                return new LoginResultDto { Success = false, Message = "Tài khoản không tồn tại" };
            }

            // Dùng user.UserName cho đăng nhập
            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, false, false);
            if (result.Succeeded)
            {
                return new LoginResultDto
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    ReturnUrl = loginDto.ReturnUrl ?? "/"
                };
            }
            else
            {
                return new LoginResultDto { Success = false, Message = "Username hoặc Password bị sai" };
            }
        }
    }
}
