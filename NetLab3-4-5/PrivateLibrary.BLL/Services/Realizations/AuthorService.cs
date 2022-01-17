using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PrivateLibrary.BLL.Common;
using PrivateLibrary.BLL.Common.Resources;
using PrivateLibrary.BLL.Common.Resources.Codes;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.DAL.Models.Book;
using PrivateLibrary.DAL.Repositories.Interfaces;

namespace PrivateLibrary.BLL.Services.Realizations
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;

        private readonly ILogger<AuthorService> _logger;
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IMapper mapper, ILogger<AuthorService> logger, IAuthorRepository authorRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _authorRepository = authorRepository;
        }

        public async Task<Result<AuthorDto?>> CreateAsync(AuthorDto? dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            var author = await _authorRepository.CreateAsync(_mapper.Map<Author>(dto));

            return Result<AuthorDto?>.Success(_mapper.Map<AuthorDto>(author));
        }

        public async Task<AuthorDto?> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<Result<AuthorDto?>> UpdateAsync(AuthorDto? dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            try
            {
                var author = await _authorRepository.UpdateAsync(_mapper.Map<Author>(dto));
                return Result<AuthorDto?>.Success(_mapper.Map<AuthorDto>(author));
            }
            catch (DBConcurrencyException)
            {
                return Result<AuthorDto?>.Failed(new OperationError { Code = BookResourceCodes.UpdateBookAuthorError, Description = Resources.BookResource.UpdateBookAuthorError });
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            await _authorRepository.RemoveAsync(new Author { Id = id }); //check
        }
    }
}
