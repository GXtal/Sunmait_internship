using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/Brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;

        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            var allBrands = await _brandRepository.GetBrands();

            var result = new List<BrandViewModel>();
            foreach (var brand in allBrands)
            {
                result.Add(new BrandViewModel { Id = brand.Id, Name = brand.Name });
            }

            return new OkObjectResult(result);
        }

        // GET api/Brands/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _brandRepository.GetBrandById(id);

            if (brand == null)
            {
                return NotFound();
            }

            var result = new BrandViewModel() { Id = brand.Id, Name = brand.Name };
            return new OkObjectResult(result);
        }

        // POST api/Brands
        [HttpPost]
        public async Task<IActionResult> AddBrand([FromBody] BrandInputModel newBrand)
        {
            bool result = await _brandRepository.AddBrand(newBrand.Name);

            if (!result)
            {
                return BadRequest();
            }

            return new OkResult();
        }

        // PUT api/Brands/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandInputModel newBrand)
        {
            bool result = await _brandRepository.UpdateBrand(id, newBrand.Name);

            if (!result)
            {
                return BadRequest();
            }

            return new OkResult();
        }

        // DELETE api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBrand(int id)
        {
            bool result = await _brandRepository.RemoveBrand(id);

            if (!result)
            {
                return BadRequest();
            }

            return new OkResult();
        }
    }
}
