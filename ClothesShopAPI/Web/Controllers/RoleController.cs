using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Role")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // GET: api/Roles
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var allRoles = await _roleService.GetRoles();

        var result = new List<RoleViewModel>();
        foreach (var role in allRoles)
        {
            result.Add(new RoleViewModel { Id = role.Id, Name = role.Name });
        }

        return new OkObjectResult(result);
    }

    // GET api/Roles/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById([FromRoute] int id)
    {
        var role = await _roleService.GetRole(id);
        var result = new RoleViewModel() { Id = role.Id, Name = role.Name };
        return new OkObjectResult(result);
    }
}
