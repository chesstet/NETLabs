using AutoMapper;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Dtos.Searching;
using PrivateLibrary.Models.Books;
using PrivateLibrary.WebApi.Models;
using PrivateLibrary.WebApi.Models.Books;

namespace PrivateLibrary.WebApi.Util.Mappings
{
    internal class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<AuthorModel, AuthorDto>().ReverseMap();
            CreateMap<BookModel, BookDto>().ReverseMap();
            CreateMap<ShortBookModel, ShortBookDto>().ReverseMap();
            CreateMap<DirectionModel, DirectionDto>().ReverseMap();
            CreateMap<BookFilter, BookFilterDto>().ReverseMap();
            CreateMap<BookFilter, ShortBookFilterDto>().ReverseMap();
            CreateMap<SearchResult<ShortBookModel>, SearchResultDto<ShortBookDto>>().ReverseMap();
            CreateMap<LibraryCustomerModel, LibraryCustomerDto>().ReverseMap();
        }
    }
}
