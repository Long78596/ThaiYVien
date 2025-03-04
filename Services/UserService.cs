using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using ThaiYVien.Data.Dto;
using ThaiYVien.Models;
using ThaiYVien.Repository.IRepository;
using ThaiYVien.Response;
using ThaiYVien.Services.IService;

namespace ThaiYVien.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ServiceResult> RegisterUserAsync(RegisterDto model)
        {
            if (model.Password.Length < 6 || !model.Password.Any(char.IsLetter) || !model.Password.Any(char.IsDigit))
            {
                return new ServiceResult(false, "Mật khẩu phải có ít nhất 6 ký tự và bao gồm cả chữ và số.");
            }

            var existingEmail = await _userRepository.FindByEmailAsync(model.Email);
            if (existingEmail != null)
            {
                return new ServiceResult(false, "Email đã tồn tại.");
            }

            var existingName = await _userRepository.FindByNameAsync(model.Username);
            if (existingName != null)
            {
                return new ServiceResult(false, "Tên tài khoản đã tồn tại.");
            }

            var newUser = new AppUserModel { UserName = model.Username, Email = model.Email };
            var result = await _userRepository.CreateUserAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                return new ServiceResult(true, "Đăng ký thành công.");
            }

            return new ServiceResult(false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

    }

}
