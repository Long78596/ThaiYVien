using Microsoft.AspNetCore.Identity;
using ThaiYVien.Models;

namespace ThaiYVien.Repository.IRepository
{
    public interface  IUserRepository
    {
        Task<AppUserModel> FindByEmailAsync(string email);
        Task<AppUserModel> FindByNameAsync(string username);
        Task<IdentityResult> CreateUserAsync(AppUserModel user, string password);
        Task<AppUserModel> FindByUsernameOrEmailAsync(string identifier);


    }
}
