using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLogic;
using Moq;
using DataBaseLogic;
using PresentationLogic.Interfaces;
using System.Windows;
namespace UnitTests
{
    [TestClass]
    public class LoginViewModelTests
    {
        [TestMethod]
        public void LogInButton_Click_WithValidCredentials_ShouldAuthenticateUser()
        {
            // Arrange
            var mockDatabaseService = new Mock<IDatabaseService>();
            var mockMessageService = new Mock<IMessageService>();
            var mockNavigationService = new Mock<INavigationService>();

            var loginWindow = new LogIn(
                mockDatabaseService.Object,
                mockMessageService.Object,
                mockNavigationService.Object);

            // Используем публичные свойства
            loginWindow.LoginTextBox.Text = "test@example.com";
            loginWindow.PasswordTextBox.Text = "password123";

            // Используем правильные параметры конструктора User
            var expectedUser = new User(
                "Иванов Иван Иванович",  // Name
                "test@example.com",      // Email
                "password123",           // Password
                "Администратор"          // Post
            );

            mockDatabaseService
                .Setup(service => service.AuthenticateUser("test@example.com", "password123"))
                .Returns(expectedUser);

            // Act
            loginWindow.LogInButton_Click(null, null);

            // Assert
            Assert.IsNotNull(loginWindow.CurrentUser);
            Assert.AreEqual("Иванов Иван Иванович", loginWindow.CurrentUser.Name); // Используем Name
            Assert.AreEqual("Администратор", loginWindow.CurrentUser.Post); // Используем Post

            mockMessageService.Verify(
                service => service.ShowMessage("Вход выполнен!"),
                Times.Once);

            mockNavigationService.Verify(
                service => service.NavigateToAddEditProduct(expectedUser),
                Times.Once);
        }

        [TestMethod]
        public void LogInButton_Click_WithEmptyFields_ShouldShowErrorMessage()
        {
            // Arrange
            var mockDatabaseService = new Mock<IDatabaseService>();
            var mockMessageService = new Mock<IMessageService>();
            var mockNavigationService = new Mock<INavigationService>();

            var loginWindow = new LogIn(
                mockDatabaseService.Object,
                mockMessageService.Object,
                mockNavigationService.Object);

            loginWindow.LoginTextBox.Text = "";
            loginWindow.PasswordTextBox.Text = "";

            // Act
            loginWindow.LogInButton_Click(null, null);

            // Assert
            Assert.IsNull(loginWindow.CurrentUser);

            mockMessageService.Verify(
                service => service.ShowMessage("Введите логин и пароль"),
                Times.Once);

            mockDatabaseService.Verify(
                service => service.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [TestMethod]
        public void LogInButton_Click_WithInvalidCredentials_ShouldShowErrorMessage()
        {
            // Arrange
            var mockDatabaseService = new Mock<IDatabaseService>();
            var mockMessageService = new Mock<IMessageService>();
            var mockNavigationService = new Mock<INavigationService>();

            var loginWindow = new LogIn(
                mockDatabaseService.Object,
                mockMessageService.Object,
                mockNavigationService.Object);

            loginWindow.LoginTextBox.Text = "wrong@example.com";
            loginWindow.PasswordTextBox.Text = "wrongpassword";

            mockDatabaseService
                .Setup(service => service.AuthenticateUser("wrong@example.com", "wrongpassword"))
                .Returns((User)null);

            // Act
            loginWindow.LogInButton_Click(null, null);

            // Assert
            Assert.IsNull(loginWindow.CurrentUser);

            mockMessageService.Verify(
                service => service.ShowMessage("Неверный логин или пароль"),
                Times.Once);

            mockNavigationService.Verify(
                service => service.NavigateToAddEditProduct(It.IsAny<User>()),
                Times.Never);
        }

        [TestMethod]
        public void SignUp_Click_ShouldCallNavigationService()
        {
            // Arrange
            var mockDatabaseService = new Mock<IDatabaseService>();
            var mockMessageService = new Mock<IMessageService>();
            var mockNavigationService = new Mock<INavigationService>();

            var loginWindow = new LogIn(
                mockDatabaseService.Object,
                mockMessageService.Object,
                mockNavigationService.Object);

            // Act
            loginWindow.SignUp_Click(null, null);

            // Assert
            mockNavigationService.Verify(
                service => service.NavigateToSignUp(loginWindow),
                Times.Once);
        }
    }
}
