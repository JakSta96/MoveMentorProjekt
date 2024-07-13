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
    public class SportTypesControllerTests
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
            context.SaveChanges();

            var mockContext = new Mock<ApplicationDbContext>(options);
            mockContext.Setup(m => m.SportType).Returns(context.SportType);
            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            return mockContext;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfSportTypes()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);
            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SportType>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);
            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenSportTypeNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);
            // Act
            var result = await controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithSportType()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);
            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SportType>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Create_RedirectsToIndex_OnSuccess()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var newSportType = new SportType { Id = 3, Name = "Tennis" };
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);
            // Act
            var result = await controller.Create(newSportType);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);
            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenSportTypeNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);
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
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);
            var sportType = new SportType { Id = 1, Name = "Updated Football" };

            // Act
            var result = await controller.Edit(1, sportType);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenSportTypeNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);

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
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new SportTypesController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
