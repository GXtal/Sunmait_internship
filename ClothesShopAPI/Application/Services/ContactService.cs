using Application.Exceptions.Messages;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IUserRepository _userRepository;

    public ContactService(IContactRepository contactRepository, IUserRepository userRepository)
    {
        _contactRepository = contactRepository;
        _userRepository = userRepository;
    }

    public async Task AddContact(int userId, string phoneNumber)
    {
        var contact = new Contact() { PhoneNumber = phoneNumber, UserId = userId };
        await _contactRepository.AddContact(contact);
    }

    public async Task<IEnumerable<Contact>> GetContacts(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var contactes = await _contactRepository.GetContactsByUser(user);
        return contactes;
    }

    public async Task RemoveContact(int id)
    {
        var contact = await _contactRepository.GetContactById(id);
        if (contact == null)
        {
            throw new NotFoundException(String.Format(ContactExceptionsMessages.ContactNotFound, id));
        }
        await _contactRepository.RemoveContact(contact);
    }
}
