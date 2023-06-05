using Application.Exceptions;
using Application.Exceptions.Messages;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class UserService : IUserService
{
    public const int UserRole = 2;

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserInfo(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, id));
        }
        return user;
    }

    public async Task<User> Login(string email, string passwordHash)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotExist, email));
        }

        if(user.PasswordHash != passwordHash)
        {
            throw new NotAuthorizedException(UserExceptionsMessages.WrongPassword);
        }

        return user;
    }

    public async Task<User> Register(string email, string passwordHash, string name, string surname)
    {
        var existingUser = await _userRepository.GetUserByEmail(email);
        if (existingUser != null)
        {
            throw new BadRequestException(String.Format(UserExceptionsMessages.UserEmailExists, email));
        }

        var user = new User() { Email = email, PasswordHash = passwordHash, Name = name, Surname = surname, RoleId = UserRole };
        user = await _userRepository.AddUser(user);
        return user;   
        
    }

    public async Task SetUserInfo(int id, string name, string surname)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, id));
        }

        user.Surname = surname;
        user.Name = name;
        await _userRepository.UpdateUser(user);
    }
}
