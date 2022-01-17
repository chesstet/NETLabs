using AutoMapper;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.DAL.Models.Book;

namespace PrivateLibrary.BLL.Infrastructure.Mappings
{
    internal class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<AuthorDto, Author>().ReverseMap();
            CreateMap<DirectionDto, Direction>().ReverseMap();
            CreateMap<LibraryCustomerDto, LibraryCustomer>().ReverseMap();
            CreateMap<ShortBookDto, Book>()
                .ForPath(dest => dest.Author!.FirstName, opt => opt.MapFrom(src => src.AuthorName))
                .ForPath(dest => dest.Direction!.Name, opt => opt.MapFrom(src => src.DirectionName))
                .ReverseMap();
        }
    }
}
