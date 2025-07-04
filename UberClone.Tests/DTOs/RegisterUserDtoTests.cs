using FluentAssertions;
using UberClone.Application.DTOs;

namespace UberClone.Tests.DTOs;

public class RegisterUserDtoTests
{
    [Fact]
    public void RegisterUserDto_ShouldInitializeWithDefaultValues()
    {
        // Act
        var dto = new RegisterUserDto();

        // Assert
        dto.Username.Should().BeNull();
        dto.Email.Should().BeNull();
        dto.Password.Should().BeNull();
        dto.FirstName.Should().BeNull();
        dto.LastName.Should().BeNull();
    }

    [Fact]
    public void RegisterUserDto_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var username = "testuser";
        var email = "test@example.com";
        var password = "password123";
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var dto = new RegisterUserDto
        {
            Username = username,
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName
        };

        // Assert
        dto.Username.Should().Be(username);
        dto.Email.Should().Be(email);
        dto.Password.Should().Be(password);
        dto.FirstName.Should().Be(firstName);
        dto.LastName.Should().Be(lastName);
    }

    [Theory]
    [InlineData("testuser1", "test1@example.com", "password123", "John", "Doe")]
    [InlineData("testuser2", "test2@example.com", "password456", "Jane", "Smith")]
    [InlineData("driver1", "driver@example.com", "driverpass", "Bob", "Johnson")]
    public void RegisterUserDto_ShouldAcceptValidData(string username, string email, string password, string firstName, string lastName)
    {
        // Act
        var dto = new RegisterUserDto
        {
            Username = username,
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName
        };

        // Assert
        dto.Username.Should().Be(username);
        dto.Email.Should().Be(email);
        dto.Password.Should().Be(password);
        dto.FirstName.Should().Be(firstName);
        dto.LastName.Should().Be(lastName);
    }

    [Fact]
    public void RegisterUserDto_ShouldHandleNullableProperties()
    {
        // Act
        var dto = new RegisterUserDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123",
            FirstName = null,
            LastName = null
        };

        // Assert
        dto.Username.Should().Be("testuser");
        dto.Email.Should().Be("test@example.com");
        dto.Password.Should().Be("password123");
        dto.FirstName.Should().BeNull();
        dto.LastName.Should().BeNull();
    }

    [Fact]
    public void RegisterUserDto_ShouldHandleEmptyStrings()
    {
        // Act
        var dto = new RegisterUserDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123",
            FirstName = "",
            LastName = ""
        };

        // Assert
        dto.Username.Should().Be("testuser");
        dto.Email.Should().Be("test@example.com");
        dto.Password.Should().Be("password123");
        dto.FirstName.Should().Be("");
        dto.LastName.Should().Be("");
    }
}
