using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PrivateLibrary.BLL.Common;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Dtos.Searching;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.Controllers;
using PrivateLibrary.Models;
using PrivateLibrary.Models.Auth;
using PrivateLibrary.Models.Books;

namespace PrivateLibrary.Tests.Controllers
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<ILogger<AuthController>> _loggerMock = new();
        private Mock<IMapper> _mapperMock = new();
        private Mock<IBookManagerService> _bookManagerServiceMock = new();
        private BookController _bookController = null!;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<AuthController>>();
            _mapperMock = new Mock<IMapper>();
            _bookManagerServiceMock = new Mock<IBookManagerService>();
            _bookController = new BookController(_mapperMock.Object, _bookManagerServiceMock.Object);
        }

        [Test]
        public async Task Cannot_Get_Book_Not_Found_Test()
        {
            // Arrange
            _bookManagerServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((BookDto?)null);

            // Act
            var result = await _bookController.Get(It.IsAny<Guid>());
            var modelErrors = _bookController.ModelState.ErrorCount;

            // Assert
            Assert.NotNull(result);
            Assert.AreNotEqual(0, modelErrors);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Can_Get_Book_Test()
        {
            // Arrange
            _bookManagerServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<BookDto>());

            // Act
            var result = await _bookController.Get(It.IsAny<Guid>());
            var modelErrors = _bookController.ModelState.ErrorCount;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, modelErrors);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Can_Get_Book_By_Filter_Test()
        {
            // Arrange
            _bookManagerServiceMock.Setup(s => s.GetByFilterAsync(It.IsAny<ShortBookFilterDto>()))
                .ReturnsAsync(It.IsAny<SearchResultDto<ShortBookDto>>());

            // Act
            var result = await _bookController.GetByFilter(It.IsAny<BookFilter>());
            var modelErrors = _bookController.ModelState.ErrorCount;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, modelErrors);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Can_Create_Book_Test()
        {
            // Arrange
            _bookManagerServiceMock.Setup(s => s.CreateAsync(It.IsAny<BookDto>()))
                .ReturnsAsync(WithSucceededResult<BookDto?>());

            // Act
            var result = await _bookController.GetByFilter(It.IsAny<BookFilter>());
            var modelErrors = _bookController.ModelState.ErrorCount;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, modelErrors);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task Cannot_Create_Book_Test()
        {
            // Arrange
            _bookManagerServiceMock.Setup(s => s.CreateAsync(It.IsAny<BookDto>()))
                .ReturnsAsync(WithNotSucceededResult<BookDto?>());

            // Act
            var result = await _bookController.GetByFilter(It.IsAny<BookFilter>());
            var modelErrors = _bookController.ModelState.ErrorCount;

            // Assert
            Assert.NotNull(result);
            Assert.AreNotEqual(0, modelErrors);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Can_Remove_Book_Test()
        {
            // Arrange
            _bookManagerServiceMock.Setup(s => s.RemoveAsync(It.IsAny<Guid>()));

            // Act
            var result = await _bookController.Remove(It.IsAny<ShortBookModel>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        public static IEnumerable<TestCaseData> BookFilterModelsTestData =>
            new List<TestCaseData>()
            {
                new TestCaseData(new BookFilter()
                {
                    AuthorFirstName = "test123",
                    AuthorLastName = "test123",
                    BookName = "A!@Jhd39HDmk3#",
                }),
                new TestCaseData(new BookFilter()
                {
                    AuthorFirstName = "test12345",
                    AuthorLastName = "test12345",
                    BookName = "A!@Jhd39HD3egh3emk3#",
                })
            };

        private static Result<T> WithSucceededResult<T>() where T: new()
        {
            return Result<T>.Success(new T());
        }

        private static Result<T> WithNotSucceededResult<T>() where T : new()
        {
            return Result<T>.Failed(new OperationError{Code = "Failed", Description = "Failed"});
        }
    }
}
