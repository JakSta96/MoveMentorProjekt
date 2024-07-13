using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MoveMentor.Controllers;
using MoveMentor.Data;
using MoveMentor.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace MoveMentor.Tests
{
    public class TreningsControllerTests
    {
        private Mock<ApplicationDbContext> GetMockedDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new ApplicationDbContext(options);

            context.SportType.AddRange(
                new SportType { Id = 1, Name = "Football" },
                new SportType { Id = 2, Name = "Basketball" }
            );

            context.Users.AddRange(
                new IdentityUser { Id = "user1", UserName = "user1@test.com" },
                new IdentityUser { Id = "user2", UserName = "user2@test.com" }
            );

            context.Trening.AddRange(
                new Trening { Id = 1, DateTimeStart = DateTime.Now, DateTimeEnd = DateTime.Now.AddHours(1), SportTypeId = 1, UserId = "user1" },
                new Trening { Id = 2, DateTimeStart = DateTime.Now, DateTimeEnd = DateTime.Now.AddHours(1), SportTypeId = 2, UserId = "user2" }
            );

            context.SaveChanges();

            var mockContext = new Mock<ApplicationDbContext>(options);
            mockContext.Setup(m => m.Trening).Returns(context.Trening);
            mockContext.Setup(m => m.SportType).Returns(context.SportType);
            mockContext.Setup(m => m.Users).Returns(context.Users);
            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            return mockContext;
        }

        private Mock<UserManager<IdentityUser>> GetMockedUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfTrenings()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("user1");
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Trening>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenTreningNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithTrening()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Trening>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Create_RedirectsToIndex_OnSuccess()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("user1");
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);
            var newTrening = new Trening { Id = 3, DateTimeStart = DateTime.Now, DateTimeEnd = DateTime.Now.AddHours(1), SportTypeId = 1, UserId = "user1" };

            // Act
            var result = await controller.Create(newTrening);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenTreningNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_RedirectsToIndex_OnSuccess()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);
            var trening = new Trening { Id = 1, DateTimeStart = DateTime.Now, DateTimeEnd = DateTime.Now.AddHours(1), SportTypeId = 1, UserId = "user1" };

            // Act
            var result = await controller.Edit(1, trening);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTreningNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndex_OnSuccess()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = GetMockedUserManager();
            var controller = new TreningsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
