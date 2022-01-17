using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PrivateLibrary.BLL.Common;
using PrivateLibrary.BLL.Dtos.Account;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.Controllers;
using PrivateLibrary.DAL.Models.User;
using PrivateLibrary.Models.Auth;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace PrivateLibrary.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<ILogger<AuthController>> _loggerMock = new();
        private Mock<IMapper> _mapperMock = new();
        private Mock<IAuthService> _authServiceMock = new();
        private Mock<ICustomerBookService> _customerBookServiceMock = new();
        private Mock<IAccountManagerService> _accountManagerServiceMock = new();
        private AuthController _authController = null!;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<AuthController>>();
            _mapperMock = new Mock<IMapper>();
            _authServiceMock = new Mock<IAuthService>();
            _customerBookServiceMock = new Mock<ICustomerBookService>();
            _accountManagerServiceMock = new Mock<IAccountManagerService>();
            _authController = new AuthController(_mapperMock.Object, _authServiceMock.Object, _customerBookServiceMock.Object, _accountManagerServiceMock.Object);
        }

        #region Register

        [TestCaseSource(nameof(RegisterViewModelsTestData))]
        public async Task Can_Register_Test(RegisterModel registerModel)
        {
            // Arrange
            _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterDto>())).ReturnsAsync(OperationResult.Success);
            _customerBookServiceMock.Setup(s => s.CreateAsync(It.IsAny<LibraryCustomerDto>())).ReturnsAsync(Result<LibraryCustomerDto?>.Success(new()));
            _accountManagerServiceMock.Setup(s => s.FindByLoginAsync(It.IsAny<string>())).ReturnsAsync(new UserDto());
            _accountManagerServiceMock
                .Setup(s => s.AddClaimAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authController.RegisterAsync(registerModel);
            var actualAction = (result as RedirectToActionResult)?.ActionName;
            var modelErrors = _authController.ModelState.ErrorCount;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, modelErrors);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual(nameof(_authController.Login), actualAction);
        }

        [TestCaseSource(nameof(RegisterViewModelsTestData))]
        public async Task Cannot_Register_Duplicate_Login_Test(RegisterModel registerModel)
        {

            // Arrange
            _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(WithFailedOperationResult());

            // Act
            var result = await _authController.RegisterAsync(registerModel);
            var modelErrors = _authController.ModelState.ErrorCount;

            // Assert
            Assert.NotNull(result);
            Assert.AreNotEqual(0, modelErrors);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCaseSource(nameof(RegisterViewModelsTestData))]
        public async Task Cannot_Register_Invalid_Input_Data_Test(RegisterModel registerModel)
        {
            // Arrange
            _authController.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = await _authController.RegisterAsync(registerModel);
            var modelErrors = _authController.ModelState.ErrorCount;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, modelErrors);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        #endregion

        #region Login

        [TestCaseSource(nameof(LoginViewModelsTestData))]
        public async Task Can_Login_Test(LoginModel loginModel)
        {
            // Arrange
            _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<LoginDto>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _authController.LoginAsync(loginModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsEmpty(_authController.ModelState);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [TestCaseSource(nameof(LoginViewModelsTestData))]
        public async Task Cannot_Login_Invalid_Input_Data_Test(LoginModel loginModel)
        {
            // Arrange
            _authController.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = await _authController.LoginAsync(loginModel);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, _authController.ModelState.ErrorCount);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        #endregion

        public static IEnumerable<TestCaseData> RegisterViewModelsTestData =>
            new List<TestCaseData>()
            {
                new TestCaseData(new RegisterModel()
                {
                    Login = "test123@gmail.com",
                    PhoneNumber = "0502391222",
                    ReturnUrl = "Return url",
                    Password = "A!@Jhd39HDmk3#",
                    RepeatPassword = "A!@Jhd39HDmk3#"
                }),
                new TestCaseData(new RegisterModel()
                {
                    Login = "test12345@gmail.com",
                    PhoneNumber = "0502395555",
                    ReturnUrl = "Return url2",
                    Password = "A!@Jhd39HD3egh3emk3#",
                    RepeatPassword = "A!@Jhd39HD3egh3emk3#"
                })
            };

        public static IEnumerable<TestCaseData> LoginViewModelsTestData =>
            new List<TestCaseData>()
            {
                new TestCaseData(new LoginModel()
                {
                    Login = "test123@gmail.com",
                    ReturnUrl = "Return url",
                    Password = "A!@Jhd39HDmk3#",
                }),
                new TestCaseData(new LoginModel()
                {
                    Login = "test12345@gmail.com",
                    ReturnUrl = "Return url2",
                    Password = "A!@Jhd39HD3egh3emk3#",
                })
            };

        private static OperationResult WithFailedOperationResult()
        {
            return OperationResult.Failed(new OperationError{Code = "Failed", Description = "Failed"});
        }
    }
}
