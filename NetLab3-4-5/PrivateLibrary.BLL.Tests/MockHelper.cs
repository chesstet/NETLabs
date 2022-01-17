using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace PrivateLibrary.BLL.Tests
{
    public static class MockHelper
    {
        public static Mock<T> CreateMockObject<T>() where T : class
        {
            return new();
        }

        public static (Mock<T1>, Mock<T2>) CreateMockObjects<T1, T2>() where T1 : class where T2 : class
        {
            return (new Mock<T1>(), new Mock<T2>());
        }

        public static (Mock<T1>, Mock<T2>, Mock<T3>) CreateMockObjects<T1, T2, T3>() where T1 : class where T2 : class where T3 : class
        {
            return (new Mock<T1>(), new Mock<T2>(), new Mock<T3>());
        }

        public static (Mock<T1>, Mock<T2>, Mock<T3>, Mock<T4>) CreateMockObjects<T1, T2, T3, T4>() where T1 : class where T2 : class where T3 : class where T4 : class
        {
            return (new Mock<T1>(), new Mock<T2>(), new Mock<T3>(), new Mock<T4>());
        }

        public static (Mock<T1>, Mock<T2>, Mock<T3>, Mock<T4>, Mock<T5>) CreateMockObjects<T1, T2, T3, T4, T5>() where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            return (new Mock<T1>(), new Mock<T2>(), new Mock<T3>(), new Mock<T4>(), new Mock<T5>());
        }

        public static Mock<T> CreateMockObject<T>(params object?[] args) where T : class
        {
            return new(args);
        }
        public static Mock<UserManager<TUser>> CreateDefaultMockUserManager<TUser>() where TUser : IdentityUser
        {
            return new(new Mock<IUserStore<TUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<TUser>>().Object,
                new IUserValidator<TUser>[0],
                new IPasswordValidator<TUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<TUser>>>().Object);
        }

        public static Mock<SignInManager<TUser>> CreateDefaultMockSignInManager<TUser>() where TUser : IdentityUser
        {
            return new(CreateDefaultMockUserManager<TUser>().Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<TUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<TUser>>().Object);
        }

        public static Mock<RoleManager<TRole>> CreateDefaultMockRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store ??= new Mock<IRoleStore<TRole>>().Object;
            var roles = new List<IRoleValidator<TRole>> { new RoleValidator<TRole>() };
            return new Mock<RoleManager<TRole>>(store, roles, null,
                new IdentityErrorDescriber(), null);
        }

    }
}
