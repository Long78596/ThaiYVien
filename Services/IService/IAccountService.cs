using ThaiYVien.Data.Dto;
using ThaiYVien.Response;

namespace ThaiYVien.Services.IService
{
    public interface IAccountService
    {
        Task<LoginResultDto> LoginAsync(LoginDto loginDto);
    }
}
