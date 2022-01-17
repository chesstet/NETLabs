using AutoMapper;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.WebApi.Models.Auth;

namespace PrivateLibrary.WebApi.Util.Mappings
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
