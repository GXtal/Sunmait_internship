using Application.Exceptions.Messages;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;

    public AddressService(IAddressRepository addressRepository, IUserRepository userRepository)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
    }

    public async Task AddAddress(int userId, string fullAddress)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var address = new Address() { FullAddress = fullAddress, UserId = userId };
        await _addressRepository.AddAddress(address);
    }

    public async Task<IEnumerable<Address>> GetAddresses(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new NotFoundException(String.Format(UserExceptionsMessages.UserNotFound, userId));
        }

        var addresses = await _addressRepository.GetAddressesByUser(user);
        return addresses;
    }

    public async Task RemoveAddress(int id)
    {
        var address = await _addressRepository.GetAddressById(id);
        if (address == null)
        {
            throw new NotFoundException(String.Format(AddressExceptionsMessages.AddressNotFound, id));
        }
        await _addressRepository.RemoveAddress(address);
    }
}
