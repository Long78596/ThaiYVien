using ThaiYVien.Data.Dto;
using ThaiYVien.Response;

namespace ThaiYVien.Services.IService
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterUserAsync(RegisterDto registerDto);
    }
}
