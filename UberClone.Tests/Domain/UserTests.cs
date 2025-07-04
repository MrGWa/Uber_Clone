using FluentAssertions;
using UberClone.Domain.Entities;

namespace UberClone.Tests.Domain;

public class UserTests
{
    [Fact]
    public void User_ShouldInitializeWithDefaultValues()
    {
        // Act
        var user = new User();

        // Assert
        user.Id.Should().NotBe(Guid.Empty);
        user.Role.Should().Be("Passenger");
        user.Username.Should().BeNull();
        user.Email.Should().BeNull();
        user.PasswordHash.Should().BeNull();
        user.FirstName.Should().BeNull();
        user.LastName.Should().BeNull();
    }

    [Fact]
    public void User_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var username = "testuser";
        var email = "test@example.com";
        var passwordHash = "hashedpassword";
        var firstName = "John";
        var lastName = "Doe";
        var role = "Driver";

        // Act
        var user = new User
        {
            Id = userId,
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            Role = role
        };

        // Assert
        user.Id.Should().Be(userId);
        user.Username.Should().Be(username);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Role.Should().Be(role);
    }

    [Theory]
    [InlineData("Passenger")]
    [InlineData("Driver")]
    [InlineData("Admin")]
    public void User_ShouldAcceptValidRoles(string role)
    {
        // Arrange & Act
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            Role = role
        };

        // Assert
        user.Role.Should().Be(role);
    }

    [Fact]
    public void User_ShouldGenerateUniqueIds()
    {
        // Arrange & Act
        var user1 = new User();
        var user2 = new User();

        // Assert
        user1.Id.Should().NotBe(user2.Id);
        user1.Id.Should().NotBe(Guid.Empty);
        user2.Id.Should().NotBe(Guid.Empty);
    }
}
