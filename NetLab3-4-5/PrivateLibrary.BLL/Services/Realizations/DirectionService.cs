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
    public class DirectionService : IDirectionService
    {
        private readonly IMapper _mapper;

        private readonly ILogger<DirectionService> _logger;
        private readonly IDirectionRepository _directionRepository;
        public DirectionService(IMapper mapper, ILogger<DirectionService> logger, IDirectionRepository directionRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _directionRepository = directionRepository;
        }

        public async Task<Result<DirectionDto?>> CreateAsync(DirectionDto? dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            var direction = await _directionRepository.CreateAsync(_mapper.Map<Direction>(dto));

            return Result<DirectionDto?>.Success(_mapper.Map<DirectionDto>(direction));
        }

        public async Task<DirectionDto?> GetByIdAsync(int id)
        {
            var author = await _directionRepository.GetByIdAsync(id);
            return _mapper.Map<DirectionDto>(author);
        }

        public async Task<Result<DirectionDto?>> UpdateAsync(DirectionDto? dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            try
            {
                var book = await _directionRepository.UpdateAsync(_mapper.Map<Direction>(dto));
                return Result<DirectionDto?>.Success(_mapper.Map<DirectionDto>(book));
            }
            catch (DBConcurrencyException)
            {
                return Result<DirectionDto?>.Failed(new OperationError { Code = BookResourceCodes.UpdateBookDirectionError, Description = Resources.BookResource.UpdateBookDirectionError });
            }
        }

        public async Task RemoveAsync(int id)
        {
            await _directionRepository.RemoveAsync(new Direction { Id = id }); //check
        }
    }
}
