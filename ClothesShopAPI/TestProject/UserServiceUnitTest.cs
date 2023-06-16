using Application.Exceptions;
using Application.Exceptions.Messages;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentAssertions;
using Moq;

namespace TestProject;

public class UserServiceUnitTest : BaseUnitTest
{
    [Fact]
    public async void Login_WrongPassword()
    {
        // Arrange
        string email = "cat@tut.by";
        string passwordHash = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f";

        _shopDbContext.Add(new User() { Email = email, PasswordHash = passwordHash, Name = "", Surname = "" });
        _shopDbContext.SaveChanges();

        SetupUserRepository();

        var userService = new UserService(_userRepository.Object);

        // Act
        // Assert
        var ex = await Assert.ThrowsAsync<NotAuthorizedException>(async () => await userService.Login(email, "wrong"));
        ex.Message.Should().Match(UserExceptionsMessages.WrongPassword);
    }

    [Fact]
    public async void Login_CorrectPassword()
    {
        // Arrange
        string email = "cat@tut.by";
        string passwordHash = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f";
        int userId = 1;

        _shopDbContext.Add(new User() { Id = userId, Email = email, PasswordHash = passwordHash, Name = "", Surname = "" });
        _shopDbContext.SaveChanges();

        SetupUserRepository();

        var userService = new UserService(_userRepository.Object);

        // Act
        var user = await userService.Login(email, passwordHash);

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be(email);
        user.Id.Should().Be(userId);
    }
}
