using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using api.Controllers;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using api.Models.DTO;

namespace api.Controllers.Tests
{
    [TestClass()]
    public class UsersControllerTests
    {
        private UsersController _controller;
        private Mock<RedSocialContext> _mockContext;
        private Mock<IConfiguration> _mockConfiguration;

        [TestInitialize]
        public void Setup()
        {
            _mockContext = new Mock<RedSocialContext>();
            _mockConfiguration = new Mock<IConfiguration>();
            _controller = new UsersController(_mockContext.Object, _mockConfiguration.Object);
        }

        [TestMethod()]
        public void UsersControllerTest()
        {
            // Esta prueba verifica que el controlador se inicializa correctamente.
            Assert.IsNotNull(_controller);
        }

        [TestMethod()]
        public void GetUserTest()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Username = "jdoe" };
            var users = new List<User> { user }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            // Act
            var result = _controller.GetUser(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(user, okResult.Value);
        }

        [TestMethod()]
        public void GetUser_NotFoundTest()
        {
            // Arrange
            var userId = 1;

            // Configura el contexto simulado para devolver nulo cuando no se encuentra el usuario
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new List<User>().AsQueryable().Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(new List<User>().AsQueryable().Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(new List<User>().AsQueryable().ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(new List<User>().AsQueryable().GetEnumerator());

            _mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            // Act
            var result = _controller.GetUser(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod()]
        public void GetTest()
        {
            // Arrange
            var users = new List<User>
    {
        new User { UserId = 1, Username = "jdoe" },
        new User { UserId = 2, Username = "asmith" }
    }.AsQueryable();

            // Configura el DbSet para devolver el IQueryable simulado
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<User>));
            Assert.AreEqual(2, ((IEnumerable<User>)result).Count());
        }



        [TestMethod()]
        public void PostTest()
        {
            // Arrange
            var userDTO = new UserDTO { Username = "jdoe", Email = "jdoe@example.com" };
            var user = new User { UserId = 1, Username = "jdoe", Email = "jdoe@example.com" };

            _mockContext.Setup(x => x.Users.Add(It.IsAny<User>())).Verifiable();
            _mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


            // Act
            var result = _controller.Post(userDTO).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


        [TestMethod()]
        public void LoginTest()
        {
            // Arrange
            var loginDTO = new LoginDTO { Email = "jdoe@example.com", Password = "hashedpassword1" };
            var user = new User { UserId = 1, Username = "jdoe", Email = "jdoe@example.com", PasswordHash = "hashedpassword1" };

            var users = new List<User> { user }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(x => x.Users).Returns(mockSet.Object);

            // Act
            var result = _controller.Login(loginDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(user, okResult.Value);
        }

    }
}
