using Microsoft.AspNetCore.Identity;
using ThaiYVien.Models;
using ThaiYVien.Repository.IRepository;

namespace ThaiYVien.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly UserManager<AppUserModel> _userManager;

        public UserRepository(UserManager<AppUserModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUserModel> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUserModel> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> CreateUserAsync(AppUserModel user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<AppUserModel> FindByUsernameOrEmailAsync(string identifier)
        {
            // Thử tìm theo username
            var user = await _userManager.FindByNameAsync(identifier);
            
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }
            return user;
        }
    }
}
