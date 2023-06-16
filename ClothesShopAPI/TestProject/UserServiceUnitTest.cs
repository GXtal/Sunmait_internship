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
        string email = faker.Internet.Email();
        string passwordHash = faker.Internet.Password();
        int roleId = faker.Random.Int(1);

        var dbName = "WrongPassword";
        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.Add(new User()
            {
                Email = email,
                RoleId = roleId,
                PasswordHash = passwordHash,
                Name = faker.Name.FirstName(),
                Surname = faker.Name.LastName()
            });
            dbContext.Add(new Role() { Id = roleId, Name = faker.Name.JobTitle() });
            dbContext.SaveChanges();

            var userService = new UserService(new UserRepository(dbContext));

            // Act
            // Assert
            var ex = await Assert.ThrowsAsync<NotAuthorizedException>(async () => await userService.Login(email, faker.Internet.Password()));
            ex.Message.Should().Match(UserExceptionsMessages.WrongPassword);
        }
    }

    [Fact]
    public async void Login_CorrectPassword()
    {
        // Arrange
        string email = faker.Internet.Email();
        string passwordHash = faker.Internet.Password();
        int userId = faker.Random.Int(1);
        int roleId = faker.Random.Int(1);

        var dbName = "CorrectPassword";
        using (var dbContext = GetInMemoryContext(dbName))
        {
            dbContext.Add(new User() { Id = userId, Email = email, RoleId = roleId, PasswordHash = passwordHash, Name = "", Surname = "" });
            dbContext.Add(new Role() { Id = roleId, Name = faker.Name.JobTitle() });
            dbContext.SaveChanges();

            var userService = new UserService(new UserRepository(dbContext));

            // Act
            var user = await userService.Login(email, passwordHash);

            // Assert
            user.Should().NotBeNull();
            user.Email.Should().Be(email);
            user.Id.Should().Be(userId);
        }
    }
}
