using Application.Mediator.Commands.AuthCommands;
using Application.Mediator.Handlers.AuthHandler;
using AutoMapper;
using Contracts.DTOs.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.AuthHandlers
{
    public class RegisterHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<RegisterHandler>> _mockLogger;
        private readonly RegisterHandler _handler;

        private const string ValidEmail = "ValidEmail@example.com";
        private const string ValidPassword = "VaildPassword";
        private const string WeakPassword = "WeakPassword";

        public RegisterHandlerTests()
        {
            var mockUserStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(
                mockUserStore.Object, null, null, null, null, null, null, null, null);

            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RegisterHandler>>();

            _handler = new RegisterHandler(
                _mockUserManager.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_RegistrationFails_ReturnsFailureResponse()
        {
            // Arrange
            var command = new RegisterCommand
            {
                RegisteredUser = new RegisterRequestDto
                {
                    Email = ValidEmail,
                    Password = WeakPassword
                }
            };

            var user = new User { Email = command.RegisteredUser.Email };

            _mockMapper.Setup(m => m.Map<User>(command.RegisteredUser)).Returns(user);
            _mockUserManager.Setup(m => m.CreateAsync(user, command.RegisteredUser.Password))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password is too weak" }));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Password is too weak", result.Errors);
        }

        [Fact]
        public async Task Handle_RoleAssignmentFailsUserDeleted_ReturnsFailureResponse()
        {
            // Arrange
            var command = new RegisterCommand
            {
                RegisteredUser = new RegisterRequestDto
                {
                    Email = ValidEmail,
                    Password = ValidPassword
                }
            };

            var user = new User { Email = command.RegisteredUser.Email };

            _mockMapper.Setup(m => m.Map<User>(command.RegisteredUser)).Returns(user);
            _mockUserManager.Setup(m => m.CreateAsync(user, command.RegisteredUser.Password))
                            .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(m => m.AddToRoleAsync(user, "User"))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role assignment failed" }));
            _mockUserManager.Setup(m => m.DeleteAsync(user))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Role assignment failed", result.Errors);
        }

        [Fact]
        public async Task Handle_SuccessfulRegistration_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new RegisterCommand
            {
                RegisteredUser = new RegisterRequestDto
                {
                    Email = ValidEmail,
                    Password = ValidPassword
                }
            };

            var user = new User { Email = command.RegisteredUser.Email };

            _mockMapper.Setup(m => m.Map<User>(command.RegisteredUser)).Returns(user);
            _mockUserManager.Setup(m => m.CreateAsync(user, command.RegisteredUser.Password))
                            .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(m => m.AddToRoleAsync(user, "User"))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

    }
}