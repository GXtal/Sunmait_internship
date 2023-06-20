using Domain.Enums;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Authorization;
using Web.AuthorizationData;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IContactService _contactService;
    private readonly IAddressService _addressService;
    private readonly TokenManager _tokenManager;

    public UserController(IUserService userService, IContactService contactService,
        IAddressService addressService, TokenManager tokenManager)
    {
        _userService = userService;
        _contactService = contactService;
        _addressService = addressService;
        _tokenManager = tokenManager;
    }

    // GET api/Users/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await _userService.GetUserInfo(id);
        var result = new UserViewModel()
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.Role.Name,
        };
        return new OkObjectResult(result);
    }

    // POST api/Users/login
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginUser([FromBody] LoginInputModel credentials)
    {
        var user = await _userService.Login(credentials.Email, credentials.PasswordHash);

        var token = _tokenManager.GenerateToken(user);
        
        var result = new TokenViewModel() { Token = token };

        return new OkObjectResult(result);
    }

    // POST api/Users/register
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterInputModel credentials)
    {
        var user = await _userService.Register(credentials.Email, credentials.PasswordHash, credentials.Name, credentials.Surname);
        var result = new UserViewModel()
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.Role.Name,
        };
        return new OkObjectResult(result);
    }

    // PUT api/Users
    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateUser([FromBody] UserInfoInput credentials)
    {
        var userId = Int32.Parse(User.FindFirst(CustomClaimNames.UserId)!.Value);
        await _userService.SetUserInfo(userId, credentials.Name, credentials.Surname);
        return new OkResult();
    }

    // POST api/Users/Contacts
    [Authorize]
    [HttpPost("Contacts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddContact([FromBody] ContactInputModel newContact)
    {
        var userId = Int32.Parse(User.FindFirst(CustomClaimNames.UserId)!.Value);
        await _contactService.AddContact(userId, newContact.PhoneNumber);
        return new OkResult();
    }

    // DELETE api/Users/Contacts/3
    [Authorize]
    [HttpDelete("Contacts/{contactId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveContact([FromRoute] int contactId)
    {
        var userId = Int32.Parse(User.FindFirst(CustomClaimNames.UserId)!.Value);
        await _contactService.RemoveContact(contactId, userId);
        return new OkResult();
    }

    // GET: api/Users/5/Contacts
    [Authorize]
    [HttpGet("{userId}/Contacts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ContactViewModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetContacts([FromRoute] int userId)
    {
        if (!(User.HasClaim(CustomClaimNames.UserId, userId.ToString())||
            User.HasClaim(CustomClaimNames.RoleId,((int)UserRole.Admin).ToString())))
        {
            return new ForbidResult();
        }

        var allContacts = await _contactService.GetContacts(userId);

        var result = new List<ContactViewModel>();
        foreach (var contact in allContacts)
        {
            result.Add(new ContactViewModel { Id = contact.Id, PhoneNumber = contact.PhoneNumber });
        }

        return new OkObjectResult(result);
    }

    // POST api/Users/Addresses
    [Authorize]
    [HttpPost("Addresses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddAddress([FromBody] AddressInputModel newAddress)
    {
        var userId = Int32.Parse(User.FindFirst(CustomClaimNames.UserId)!.Value);
        await _addressService.AddAddress(userId, newAddress.FullAddress);
        return new OkResult();
    }

    // DELETE api/Users/Addresses/3
    [Authorize]
    [HttpDelete("Addresses/{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveAddress([FromRoute] int addressId)
    {
        var userId = Int32.Parse(User.FindFirst(CustomClaimNames.UserId)!.Value);
        await _addressService.RemoveAddress(addressId, userId);
        return new OkResult();
    }

    // GET: api/Users/5/Addresses
    [HttpGet("{userId}/Addresses")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AddressViewModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAddresses([FromRoute] int userId)
    {
        if (!(User.HasClaim(CustomClaimNames.UserId, userId.ToString()) ||
            User.HasClaim(CustomClaimNames.RoleId, ((int)UserRole.Admin).ToString())))
        {
            return new ForbidResult();
        }

        var allAddresses = await _addressService.GetAddresses(userId);

        var result = new List<AddressViewModel>();
        foreach (var address in allAddresses)
        {
            result.Add(new AddressViewModel { Id = address.Id, FullAddress = address.FullAddress });
        }

        return new OkObjectResult(result);
    }
}
