using AutoMapper;
using PrivateLibrary.BLL.Dtos.Account;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.DAL.Models.User;

namespace PrivateLibrary.WebApi.Util.Mappings
{
    internal class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterDto, UserCreationDto>();
        }
    }
}
