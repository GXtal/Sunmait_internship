using Application.Exceptions;
using Application.Exceptions.Messages;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace TestProject;

public class UserServiceUnitTest
{
    Mock<IUserRepository> _userRepository;

    public UserServiceUnitTest()
    {
        _userRepository = new Mock<IUserRepository>();
    }

    [Fact]
    public async void Login_WrongPasswordTest()
    {
        // Arrange
        string email = "cat@tut.by";
        string passwordHash = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f";
        int userId = 1;
        _userRepository.Setup(x => x.GetUserByEmail(email)).ReturnsAsync(new User() { Id = userId, Email = email, PasswordHash = passwordHash });

        var userService = new UserService(_userRepository.Object);

        // Act
        try
        {
            await userService.Login(email, "wrong");
        }
        catch (Exception ex)
        {
            // Assert
            ex.Should().BeOfType<NotAuthorizedException>();
            ex.Message.Should().Match(UserExceptionsMessages.WrongPassword);
        }
    }

    [Fact]
    public async void Login_CorrectPasswordTest()
    {
        // Arrange
        string email = "cat@tut.by";
        string passwordHash = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f";
        int userId = 1;
        _userRepository.Setup(x => x.GetUserByEmail(email)).ReturnsAsync(new User() { Id = userId, Email = email, PasswordHash = passwordHash });

        var userService = new UserService(_userRepository.Object);

        // Act
        var user = await userService.Login(email, passwordHash);

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be(email);
        user.Id.Should().Be(userId);
    }
}
