using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.BLL.Services.Realizations;
using PrivateLibrary.DAL.Models.User;

namespace PrivateLibrary.BLL.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<ILogger<AuthService>> _loggerMock = new();
        private Mock<IMapper> _mapperMock = new();
        private Mock<SignInManager<User>> _signInManagerMock = new();
        private Mock<IAccountManagerService> _accountManagerServiceMock = new();
        private Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private Mock<IUserClaimsPrincipalFactory<User>> _userClaimsPrincipalFactoryMock = new();
        private Mock<UserManager<User>> _userManagerMock = new();
        private IAuthService _authService = null!;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IUserStore<User>>().Object;
            _userManagerMock = MockHelper.CreateMockObject<UserManager<User>>(store,null,null, null, null, null, null, null, null);
            _httpContextAccessorMock = MockHelper.CreateMockObject<IHttpContextAccessor>();
            _userClaimsPrincipalFactoryMock = MockHelper.CreateMockObject<IUserClaimsPrincipalFactory<User>>();
            _accountManagerServiceMock = MockHelper.CreateMockObject<IAccountManagerService>();
            _signInManagerMock = MockHelper.CreateMockObject<SignInManager<User>>(_userManagerMock.Object, _httpContextAccessorMock.Object, _userClaimsPrincipalFactoryMock.Object, null, null, null, null);
            _mapperMock = MockHelper.CreateMockObject<IMapper>();
            _loggerMock = MockHelper.CreateMockObject<ILogger<AuthService>>();
            _authService = new AuthService(
                _mapperMock.Object, 
                _loggerMock.Object, 
                _signInManagerMock.Object, 
                _accountManagerServiceMock.Object,
                _httpContextAccessorMock.Object
                );
        }

        [Test]
        public async Task Cannot_Register_User_Already_Exists_Test()
        {
            // Arrange
            _accountManagerServiceMock.Setup(s => s.IsUserWithSuchLoginAlreadyExist(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _authService.RegisterAsync(It.IsAny<RegisterDto>());

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(false, result.Succeeded);
        }

        public static IEnumerable<TestCaseData> LoginViewModelsTestData =>
            new List<TestCaseData>()
            {
                new TestCaseData(new LoginDto()
                {
                    Login = "test123",
                    Password = "A!@Jhd39HDmk3#",
                }),
                new TestCaseData(new LoginDto()
                {
                    Login = "test12345",
                    Password = "A!@Jhd39HD3egh3emk3#",
                })
            };

        public static IEnumerable<TestCaseData> RegisterViewModelsTestData =>
            new List<TestCaseData>()
            {
                new TestCaseData(new LoginDto()
                {
                    Login = "test123",
                    Password = "A!@Jhd39HDmk3#",
                }),
                new TestCaseData(new LoginDto()
                {
                    Login = "test12345",
                    Password = "A!@Jhd39HD3egh3emk3#",
                })
            };
    }
}
