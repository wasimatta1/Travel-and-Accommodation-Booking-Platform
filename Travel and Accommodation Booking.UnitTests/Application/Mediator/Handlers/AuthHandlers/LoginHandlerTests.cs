using Application.Mediator.Commands.AuthCommands;
using Application.Mediator.Handlers.AuthHandler;
using Contracts.DTOs.Authentication;
using Contracts.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.AuthHandlers
{
    public class LoginHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ILogger<LoginHandler>> _mockLogger;
        private readonly LoginHandler _handler;

        private const string ValidEmail = "ValidEmail@example.com";
        private const string ValidPassword = "VaildPassword";
        private const string InvalidPassword = "InvalidPassword";
        private const string Token = "MockedToken";

        public LoginHandlerTests()
        {
            var mockUserStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(mockUserStore.Object,
                null, null, null, null, null, null, null, null);

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var mockClaimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, mockContextAccessor.Object,
                mockClaimsFactory.Object, null, null, null, null);

            _mockTokenService = new Mock<ITokenService>();

            _mockLogger = new Mock<ILogger<LoginHandler>>();

            _handler = new LoginHandler(
                _mockUserManager.Object,
                _mockTokenService.Object,
                _mockSignInManager.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsFailureResponse()
        {
            // Arrange
            var command = new LoginCommand
            {
                LoginRequest = new LoginRequestDto
                {
                    Email = ValidEmail,
                    Password = ValidPassword
                }
            };
            _mockUserManager.Setup(m => m.FindByEmailAsync(ValidEmail))
                            .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ReturnsFailureResponse()
        {
            // Arrange
            var user = new User { Email = ValidEmail };
            var command = new LoginCommand
            {
                LoginRequest = new LoginRequestDto
                {
                    Email = ValidEmail,
                    Password = InvalidPassword
                }
            };

            _mockUserManager.Setup(m => m.FindByEmailAsync(ValidEmail)).ReturnsAsync(user);
            _mockSignInManager.Setup(s => s.CheckPasswordSignInAsync(user, InvalidPassword, false))
                              .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ValidLogin_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new User { Email = ValidEmail };
            var command = new LoginCommand
            {
                LoginRequest = new LoginRequestDto
                {
                    Email = ValidEmail,
                    Password = ValidPassword
                }
            };

            var roles = new[] { "Admin" };

            _mockUserManager.Setup(m => m.FindByEmailAsync(ValidEmail)).ReturnsAsync(user);
            _mockSignInManager.Setup(s => s.CheckPasswordSignInAsync(user, ValidPassword, false))
                              .ReturnsAsync(SignInResult.Success);
            _mockUserManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);
            _mockTokenService.Setup(t => t.GenerateTokenAsync(ValidEmail, roles[0])).ReturnsAsync(Token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(Token, result.Token);
        }
    }
}