using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.Config.RequestLimits;
using PrivateLibrary.Controllers;

namespace PrivateLibrary.Tests.Controllers
{
    [TestFixture]
    public class CustomerBooksControllerTests
    {
        private Mock<IMapper> _mapperMock = new();
        private Mock<ICustomerBookService> _customerBooksManagerServiceMock = new();
        private Mock<IOptions<BookLimits>> _bookLimitsMock = new();
        private CustomerBooksController _customerBooksController = null!;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _customerBooksManagerServiceMock = new Mock<ICustomerBookService>();
            _bookLimitsMock = new Mock<IOptions<BookLimits>>();
            _customerBooksController = new CustomerBooksController(_mapperMock.Object, _customerBooksManagerServiceMock.Object, _bookLimitsMock.Object);
        }
    }
}
