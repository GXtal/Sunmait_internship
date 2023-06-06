using Application.Services;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IContactService _contactService;
    private readonly IAddressService _addressService;

    public UserController(IUserService userService, IOrderService orderService,
        IContactService contactService, IAddressService addressService)
    {
        _userService = userService;
        _orderService = orderService;
        _contactService = contactService;
        _addressService = addressService;
    }

    // GET api/Users/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await _userService.GetUserInfo(id);
        var result = new UserViewModel() { Id = user.Id, Name = user.Name, Surname = user.Surname, Email = user.Email, RoleId = user.RoleId };
        return new OkObjectResult(result);
    }

    // POST api/Users/login
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginInputModel credentials)
    {
        var user = await _userService.Login(credentials.Email, credentials.PasswordHash);
        var result = new UserViewModel() { Id = user.Id, Name = user.Name, Surname = user.Surname, Email = user.Email, RoleId = user.RoleId };
        return new OkObjectResult(result);
    }

    // POST api/Users/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterInputModel credentials)
    {
        var user = await _userService.Register(credentials.Email, credentials.PasswordHash, credentials.Name, credentials.Surname);
        var result = new UserViewModel() { Id = user.Id, Name = user.Name, Surname = user.Surname, Email = user.Email, RoleId = user.RoleId };
        return new OkObjectResult(result);
    }

    // PUT api/Users/register
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserInfoInput credentials)
    {
        await _userService.SetUserInfo(id, credentials.Name, credentials.Surname);
        return new OkResult();
    }

    // GET: api/Users/5/Orders
    [HttpGet("{userId}/Orders")]
    public async Task<IActionResult> GetOrders([FromRoute] int userId)
    {
        var allOrders = await _orderService.GetOrders(userId);

        var result = new List<OrderViewModel>();
        foreach (var order in allOrders)
        {
            result.Add(new OrderViewModel { Id = order.Id, TotalCost = order.TotalCost, UserId = order.UserId });
        }

        return new OkObjectResult(result);
    }

    // POST api/Users/5/Orders
    [HttpPost("{userId}/Orders")]
    public async Task<IActionResult> AddOrder([FromRoute] int userId)
    {
        var order = await _orderService.AddOrder(userId);
        var result = new OrderViewModel() { Id = order.Id, TotalCost = order.TotalCost, UserId = order.UserId };
        return new OkObjectResult(result);
    }

    // POST api/Users/5/Contacts
    [HttpPost("{userId}/Contacts")]
    public async Task<IActionResult> AddContact([FromRoute] int userId, [FromBody] ContactInputModel newContact)
    {
        await _contactService.AddContact(userId, newContact.PhoneNumber);
        return new OkResult();
    }

    // DELETE api/Users/5/Contacts/3
    [HttpDelete("{userId}/Contacts/{contactId}")]
    public async Task<IActionResult> RemoveContact([FromRoute] int contactId)
    {
        await _contactService.RemoveContact(contactId);
        return new OkResult();
    }

    // GET: api/Users/5/Contacts
    [HttpGet("{userId}/Contacts")]
    public async Task<IActionResult> GetContacts([FromRoute] int userId)
    {
        var allContacts = await _contactService.GetContacts(userId);

        var result = new List<ContactViewModel>();
        foreach (var contact in allContacts)
        {
            result.Add(new ContactViewModel { Id = contact.Id, PhoneNumber = contact.PhoneNumber });
        }

        return new OkObjectResult(result);
    }

    // POST api/Users/5/Addresses
    [HttpPost("{userId}/Addresses")]
    public async Task<IActionResult> AddAddress([FromRoute] int userId, [FromBody] AddressInputModel newAddress)
    {
        await _addressService.AddAddress(userId, newAddress.FullAddress);
        return new OkResult();
    }

    // DELETE api/Users/5/Addresses/3
    [HttpDelete("{userId}/Addresses/{addressId}")]
    public async Task<IActionResult> RemoveAddress([FromRoute] int addressId)
    {
        await _addressService.RemoveAddress(addressId);
        return new OkResult();
    }

    // GET: api/Users/5/Addresses
    [HttpGet("{userId}/Addresses")]
    public async Task<IActionResult> GetAddresses([FromRoute] int userId)
    {
        var allAddresses = await _addressService.GetAddresses(userId);

        var result = new List<AddressViewModel>();
        foreach (var address in allAddresses)
        {
            result.Add(new AddressViewModel { Id = address.Id, FullAddress = address.FullAddress });
        }

        return new OkObjectResult(result);
    }
}
