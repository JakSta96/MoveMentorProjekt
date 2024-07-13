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
    public class ContactsControllerTests
    {
        private Mock<ApplicationDbContext> GetMockedDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new ApplicationDbContext(options);

            context.Contacts.AddRange(
                new Contacts { Id = 1, Name = "John Doe", Phone = "1234567890", mail = "john.doe@example.com", UserId = "test-user-id" },
                new Contacts { Id = 2, Name = "Jane Doe", Phone = "0987654321", mail = "jane.doe@example.com", UserId = "test-user-id" }
            );
            context.SaveChanges();

            var mockContext = new Mock<ApplicationDbContext>(options);
            mockContext.Setup(m => m.Contacts).Returns(context.Contacts);
            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            return mockContext;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfContacts()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("test-user-id");
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);
            
            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Contacts>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);
            
            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenContactNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("test-user-id");
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);
            
            // Act
            var result = await controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithContact()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("test-user-id");
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);
            
            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Contacts>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Create_RedirectsToIndex_OnSuccess()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("test-user-id");
            var newContact = new Contacts { Id = 3, Name = "Tennis" };
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);
            
            // Act
            var result = await controller.Create(newContact);

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
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);
            
            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenContactNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("test-user-id");
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);
            
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
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("test-user-id");
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);
            var contact = new Contacts { Id = 1, Name = "Updated John Doe", Phone = "1234567890", mail = "john.doe@example.com", UserId = "test-user-id" };

            // Act
            var result = await controller.Edit(1, contact);

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
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenContactNotFound()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("test-user-id");
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);

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
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("test-user-id");
            var controller = new ContactsController(mockContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
