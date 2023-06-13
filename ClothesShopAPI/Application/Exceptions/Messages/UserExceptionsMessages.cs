namespace Application.Exceptions.Messages;

public class UserExceptionsMessages
{
    public const string UserNotFound = "User with id={0} is not found";
    public const string UserNotExist = "User with email=\"{0}\" does not exist";
    public const string UserEmailExists = "User with email=\"{0}\" already exists";
    public const string WrongPassword = "Invalid password";
}
