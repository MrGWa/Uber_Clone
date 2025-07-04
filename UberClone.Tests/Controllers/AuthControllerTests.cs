using Microsoft.AspNetCore.Mvc;
using UberClone.Tests.TestControllers;

namespace UberClone.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IRegisterUserCommand> _mockRegisterUserCommand;
    private readonly TestAuthController _controller;

    public AuthControllerTests()
    {
        _mockRegisterUserCommand = new Mock<IRegisterUserCommand>();
        _controller = new TestAuthController(_mockRegisterUserCommand.Object);
    }

    [Fact]
    public async Task Register_WithValidDto_ReturnsOkResult()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Username = "test_user",
            Email = "test@example.com",
            Password = "password123",
            FirstName = "John",
            LastName = "Doe"
        };

        _mockRegisterUserCommand
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterUserDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().Be("User registered successfully.");
        
        _mockRegisterUserCommand.Verify(x => x.ExecuteAsync(registerDto), Times.Once);
    }

    [Fact]
    public async Task Register_WhenExceptionThrown_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Username = "test_user",
            Email = "test@example.com",
            Password = "password123"
        };

        var exceptionMessage = "User already exists";
        _mockRegisterUserCommand
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterUserDto>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Be(exceptionMessage);
    }

    [Fact]
    public async Task Register_WithNullDto_ShouldHandleGracefully()
    {
        // Arrange
        RegisterUserDto? nullDto = null;

        _mockRegisterUserCommand
            .Setup(x => x.ExecuteAsync(It.IsAny<RegisterUserDto>()))
            .ThrowsAsync(new ArgumentNullException());

        // Act
        var result = await _controller.Register(nullDto!);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
