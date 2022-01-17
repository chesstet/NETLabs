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
    public class CustomerBookService : ICustomerBookService
    {
        private readonly IMapper _mapper;

        private readonly ILogger<DirectionService> _logger;
        private readonly ICustomerBookRepository _customerBookRepository;
        private readonly IBookRepository _bookRepository;
        public CustomerBookService(IMapper mapper, ILogger<DirectionService> logger, ICustomerBookRepository customerBookRepository, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _customerBookRepository = customerBookRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Result<LibraryCustomerDto?>> CreateAsync(LibraryCustomerDto? dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            var customer = await _customerBookRepository.CreateAsync(_mapper.Map<LibraryCustomer>(dto));

            return Result<LibraryCustomerDto?>.Success(_mapper.Map<LibraryCustomerDto>(customer));
        }

        public async Task<LibraryCustomerDto?> GetByIdAsync(Guid id)
        {
            var customer = await _customerBookRepository.GetByIdAsync(id);
            return _mapper.Map<LibraryCustomerDto>(customer);
        }

        public async Task<Result<LibraryCustomerDto?>> UpdateAsync(LibraryCustomerDto? dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            try
            {
                var book = await _customerBookRepository.UpdateAsync(_mapper.Map<LibraryCustomer>(dto));
                return Result<LibraryCustomerDto?>.Success(_mapper.Map<LibraryCustomerDto>(book));
            }
            catch (DBConcurrencyException)
            {
                return Result<LibraryCustomerDto?>.Failed(new OperationError { Code = BookResourceCodes.UpdateBookCustomerError, Description = Resources.BookResource.UpdateBookCustomerError });
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            await _customerBookRepository.RemoveAsync(new LibraryCustomer { Id = id }); //check
        }

        public async Task<OperationResult> AddBookForCustomer(Guid customerId, Guid bookId)
        {
            var customer = await _customerBookRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return OperationResult.Failed(new OperationError{Code = BookResourceCodes.CustomerNotFoundError, Description = Resources.BookResource.CustomerNotFoundError});
            }

            var bookCollection = customer.Books;

            if (bookCollection == null)
            {
                throw new InvalidOperationException();
            }

            var bookAlreadyExist = bookCollection.FirstOrDefault(x => x.Id == bookId) != null;
            if (!bookAlreadyExist)
            {
                var book = await _bookRepository.GetByIdAsync(bookId);

                if (book != null)
                {
                    await _customerBookRepository.AddBookForCustomer(customer, book);
                }
                return OperationResult.Success;
            }

            return OperationResult.Failed(new OperationError{Code = BookResourceCodes.BookForCustomerAlreadyExistsError, Description = Resources.BookResource.BookForCustomerAlreadyExistsError});
        }

        public async Task<OperationResult> RemoveBookForCustomer(Guid customerId, Guid bookId)
        {
            var customer = await _customerBookRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return OperationResult.Failed(new OperationError { Code = BookResourceCodes.CustomerNotFoundError, Description = Resources.BookResource.CustomerNotFoundError });
            }

            var bookCollection = customer.Books;

            if (bookCollection == null)
            {
                throw new InvalidOperationException();
            }

            var bookExists = bookCollection.FirstOrDefault(x => x.Id == bookId) != null;
            if (bookExists)
            {
                var book = await _bookRepository.GetByIdAsync(bookId);

                if (book != null)
                {
                    await _customerBookRepository.RemoveBookForCustomer(customer, book);
                }
                return OperationResult.Success;
            }

            return OperationResult.Failed(new OperationError { Code = BookResourceCodes.BookForCustomerNotFoundError, Description = Resources.BookResource.BookForCustomerNotFoundError});
        }
    }
}
