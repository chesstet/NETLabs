using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrivateLibrary.BLL.Common;
using PrivateLibrary.BLL.Common.Resources;
using PrivateLibrary.BLL.Common.Resources.Codes;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Dtos.Searching;
using PrivateLibrary.BLL.Infrastructure.Enums.Filters.Books;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.BLL.Util;
using PrivateLibrary.DAL.Models.Book;
using PrivateLibrary.DAL.Repositories.Interfaces;

namespace PrivateLibrary.BLL.Services.Realizations
{
    public class BookManagerService : IBookManagerService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BookManagerService> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorService _authorService;
        private readonly IDirectionService _directionService;
        public BookManagerService(IMapper mapper, ILogger<BookManagerService> logger, IBookRepository bookRepository, IAuthorService authorService, IDirectionService directionService)
        {
            _mapper = mapper;
            _logger = logger;  
            _bookRepository = bookRepository;
            _authorService = authorService;
            _directionService = directionService;
        }
        public async Task<Result<BookDto?>> CreateAsync(BookDto? dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            dto.Id = Guid.Empty;

            var authorDto = await RetrievingAuthorDtoData(dto.Author);

            if (authorDto == null)
            {
                return Result<BookDto?>.Failed(new OperationError{Code = BookResourceCodes.UnrealToRetrieveAuthorError, Description = Resources.BookResource.UnrealToRetrieveAuthorError});
            }

            var directionDto = await RetrievingDirectionDtoData(dto.Direction);

            if (directionDto == null)
            {
                return Result<BookDto?>.Failed(new OperationError { Code = BookResourceCodes.UnrealToRetrieveDirectionError, Description = Resources.BookResource.UnrealToRetrieveDirectionError });
            }

            dto.Author = null;
            dto.Direction = null;
            var mappedBook = _mapper.Map<BookDto, Book>(dto);
            mappedBook.AuthorId = authorDto.Id;
            mappedBook.DirectionId = directionDto.Id;

            var book = await _bookRepository.CreateAsync(mappedBook);

            return Result<BookDto?>.Success(_mapper.Map<BookDto>(book));
        }

        public async Task<BookDto?> GetByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<Result<BookDto?>> UpdateAsync(BookDto? dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            try
            {
                var book = await _bookRepository.UpdateAsync(_mapper.Map<Book>(dto));
                return Result<BookDto?>.Success(_mapper.Map<BookDto>(book));
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<BookDto?>.Failed(new OperationError{ Code = BookResourceCodes.UpdateBookError, Description = Resources.BookResource.UpdateBookError});
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            await _bookRepository.RemoveAsync(new Book {Id = id}); //check
        }

        public async Task<IEnumerable<ShortBookDto>?> GetByAuthorIdAsync(Guid id)
        {
            var books = await _bookRepository.GetByAuthorIdAsync(id);

            return _mapper.Map<List<ShortBookDto>>(books);
        }

        public async Task<IEnumerable<ShortBookDto>?> GetByLibraryCustomerIdAsync(Guid id)
        {
            var books = await _bookRepository.GetByLibraryCustomerIdAsync(id);

            return _mapper.Map<IEnumerable<ShortBookDto>?>(books);
        }

        public async Task<SearchResultDto<ShortBookDto>> GetByFilterAsync(ShortBookFilterDto? filterDto)
        {
            filterDto ??= new ShortBookFilterDto();

            var filterPredicate = PredicateBuild(filterDto);

            var results = await _bookRepository.Get<dynamic>(
                    skip: filterDto.From,
                    take: filterDto.Size,
                    where: filterPredicate,
                    orderBy: x => x.Name,
                    ascending: true)
                .Include(x => x.Author)
                .Include(x => x.Direction)
                .ToListAsync();

            return new SearchResultDto<ShortBookDto>()
            {
                TotalAmount = await _bookRepository.CountAsync(where: filterPredicate),
                Entities = _mapper.Map<List<ShortBookDto>>(results)
            };
        }

        private static Expression<Func<Book, bool>> PredicateBuild(ShortBookFilterDto filter)
        {
            var predicate = PredicateBuilder.True<Book>();

            if (filter.AuthorIds.Any())
            {
                predicate = predicate.And(x => filter.AuthorIds.Any(c => c == x.Id));

                return predicate;
            }

            if (!string.IsNullOrEmpty(filter.AuthorFirstName))
            {
                predicate = predicate.And(x => x.Author != null && x.Author.FirstName == filter.AuthorFirstName);
            }

            if (!string.IsNullOrEmpty(filter.AuthorLastName))
            {
                predicate = predicate.And(x => x.Author != null && x.Author.LastName == filter.AuthorLastName);
            }

            if (!string.IsNullOrEmpty(filter.BookName))
            {
                predicate = predicate.And(x => x.Name == filter.BookName);
            }

            if (!string.IsNullOrEmpty(filter.DirectionName))
            {
                predicate = predicate.And(x => x.Direction != null && x.Direction.Name == filter.DirectionName);
            }

            if (filter.BookYear != 0)
            {
                predicate = predicate.And(x => x.Year == filter.BookYear);
            }

            return predicate;
        }

        private async Task<AuthorDto?> RetrievingAuthorDtoData(AuthorDto? authorDto)
        {
            ArgumentNullException.ThrowIfNull(authorDto);
            if (authorDto.Id == default)
            {
                var newAuthorDto = await _authorService.CreateAsync(authorDto);
                return newAuthorDto is { Succeeded: true } ? newAuthorDto.Value : null;
            }

            return await _authorService.GetByIdAsync(authorDto.Id);
        }

        private async Task<DirectionDto?> RetrievingDirectionDtoData(DirectionDto? directionDto)
        {
            ArgumentNullException.ThrowIfNull(directionDto);
            if (directionDto.Id == default)
            {
                var newDirectionDto = await _directionService.CreateAsync(directionDto);
                return newDirectionDto is { Succeeded: true } ? newDirectionDto.Value : null;
            }

            return await _directionService.GetByIdAsync(directionDto.Id);
        }
    }
}
