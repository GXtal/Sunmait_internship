using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces.Services;
using Web.Models.InputModels;
using Web.Models.ViewModels;

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

    // GET: api/Brands
    [HttpGet]
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

    // GET api/Brands/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandById([FromRoute] int id)
    {
        var brand = await _brandService.GetBrand(id);
        var result = new BrandViewModel() { Id = brand.Id, Name = brand.Name };
        return new OkObjectResult(result);
    }

    // POST api/Brands
    [HttpPost]
    public async Task<IActionResult> AddBrand([FromBody] BrandInputModel newBrand)
    {
        await _brandService.AddBrand(newBrand.Name);
        return new OkResult();
    }

    // PUT api/Brands/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBrand([FromRoute] int id, [FromBody] BrandInputModel newBrand)
    {
        await _brandService.RenameBrand(id, newBrand.Name);
        return new OkResult();
    }

    // DELETE api/Brands/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveBrand([FromRoute] int id)
    {
        await _brandService.RemoveBrand(id);
        return new OkResult();
    }
}
