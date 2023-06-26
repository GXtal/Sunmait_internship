using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces.Services;
using Web.Models.InputModels;
using Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Web.Authorization;
using Web.AuthorizationData;
using Domain.Enums;

namespace Web.Controllers;

[Route("api/Brands")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    // GET api/Brands
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandViewModel>))]
    public async Task<IActionResult> GetBrands()
    {
        var allBrands = await _brandService.GetBrands();

        var result = new List<BrandViewModel>();
        foreach (var brand in allBrands)
        {
            result.Add(new BrandViewModel { Id = brand.Id, Name = brand.Name });
        }

        return new OkObjectResult(result);
    }

    // POST api/Brands
    [Authorize]
    [RequiresClaim(CustomClaimNames.RoleId, (int)UserRole.Admin)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddBrand([FromBody] BrandInputModel newBrand)
    {
        await _brandService.AddBrand(newBrand.Name);
        return new OkResult();
    }

    // PUT api/Brands/5
    [Authorize]
    [RequiresClaim(CustomClaimNames.RoleId, (int)UserRole.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateBrand([FromRoute] int id, [FromBody] BrandInputModel newBrand)
    {
        await _brandService.RenameBrand(id, newBrand.Name);
        return new OkResult();
    }

    // DELETE api/Brands/5
    [Authorize]
    [RequiresClaim(CustomClaimNames.RoleId, (int)UserRole.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveBrand([FromRoute] int id)
    {
        await _brandService.RemoveBrand(id);
        return new OkResult();
    }
}
