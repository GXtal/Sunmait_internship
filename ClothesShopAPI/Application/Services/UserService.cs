using Application.Exceptions;
using Application.Exceptions.Messages;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

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

    private static string ComputeSha256Hash(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    public async Task<User> Login(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotExist, email));
        }

        var passwordHash = ComputeSha256Hash(password);

        if (user.PasswordHash != passwordHash)
        {
            throw new NotAuthorizedException(UserExceptionsMessages.WrongPassword);
        }

        return user;
    }

    public async Task<User> Register(string email, string password, string name, string surname)
    {
        var existingUser = await _userRepository.GetUserByEmail(email);
        if (existingUser != null)
        {
            throw new BadRequestException(String.Format(UserExceptionsMessages.UserEmailExists, email));
        }

        var passwordHash = ComputeSha256Hash(password);

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
