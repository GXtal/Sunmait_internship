using Application.Exceptions;
using Application.Exceptions.Messages;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;

namespace TestProject;

public class UserServiceUnitTest : BaseUnitTest
{
    [Fact]
    public async void Login_WrongPassword()
    {
        // Arrange
        var dbName = "WrongPassword";
        using (var dbContext = GetInMemoryContext(dbName))
        {
            var user = AddUser(dbContext);
            dbContext.SaveChanges();

            var userService = new UserService(new UserRepository(dbContext));

            // Act
            // Assert
            var ex = await Assert.ThrowsAsync<NotAuthorizedException>(async () => await userService.Login(user.Email, faker.Internet.Password()));
            ex.Message.Should().Match(UserExceptionsMessages.WrongPassword);
        }
    }

    [Fact]
    public async void Login_CorrectPassword()
    {
        // Arrange

        var dbName = "CorrectPassword";
        using (var dbContext = GetInMemoryContext(dbName))
        {
            var added = AddUser(dbContext);
            dbContext.SaveChanges();

            var userService = new UserService(new UserRepository(dbContext));

            // Act
            var user = await userService.Login(added.Email, added.PasswordHash);

            // Assert
            user.Should().NotBeNull();
            user.Email.Should().Be(added.Email);
            user.Id.Should().Be(added.Id);
        }
    }
}
