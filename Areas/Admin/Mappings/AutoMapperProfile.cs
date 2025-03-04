
using ThaiYVien.Areas.Admin.Dto.User;
using ThaiYVien.Models;
using AutoMapper;

namespace ThaiYVien.Areas.Admin.Mappings
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<AppUserModel, UserResponseDto>()
					   .ForMember(dest => dest.RoleName, opt => opt.Ignore());

			CreateMap<UserCreateDto, AppUserModel>();
			CreateMap<UserUpdateDto, AppUserModel>();
		}
	}
}
