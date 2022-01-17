using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.BLL.Services.Realizations;
using PrivateLibrary.DAL.Repositories.Interfaces;

namespace PrivateLibrary.BLL.Tests.Services
{
    [TestFixture]
    public class BookManagerServiceTests
    {
        private Mock<IMapper> _mapperMock = new();
        private Mock<ILogger<BookManagerService>> _loggerMock = new();
        private Mock<IBookRepository> _bookRepositoryMock = new();
        private Mock<IAuthorService> _authorServiceMock = new();
        private Mock<IDirectionService> _directionServiceMock = new();
        private IBookManagerService _bookManagerService =null!;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<BookManagerService>>();
            _mapperMock = new Mock<IMapper>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorServiceMock = new Mock<IAuthorService>();
            _directionServiceMock = new Mock<IDirectionService>();
            _directionServiceMock = new Mock<IDirectionService>();
            _bookManagerService = new BookManagerService(
                _mapperMock.Object,
                _loggerMock.Object,
                _bookRepositoryMock.Object,
                _authorServiceMock.Object,
                _directionServiceMock.Object);
        }

        [Test]
        public async Task Can_CreateAsync_Test()
        {
            
        }
    }
}
