using AutoMapper;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.Models.Auth;

namespace PrivateLibrary.Util.Mappings
{
    internal class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<LoginModel, LoginDto>().ReverseMap();
            CreateMap<RegisterModel, RegisterDto>().ReverseMap();
        }
    }
}
