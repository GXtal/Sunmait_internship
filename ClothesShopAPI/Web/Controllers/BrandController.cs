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
        public async Task<IActionResult> Get()
        {
            var allBrands = await _brandRepository.GetBrands();

            var allBrandsSimplified = new List<BrandViewModel>();
            foreach (var brand in allBrands)
            {
                allBrandsSimplified.Add(new BrandViewModel { Id = brand.Id, Name = brand.Name });
            }

            return new OkObjectResult(allBrandsSimplified);
        }

        // GET api/Brands/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var brand = await _brandRepository.GetBrandById(id);

            if (brand == null)
            {
                return NotFound();
            }

            var brandSimplified = new BrandViewModel() { Id = brand.Id, Name = brand.Name };
            return new OkObjectResult(brandSimplified);
        }

        // POST api/Brands
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BrandInputModel newBrand)
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
        public async Task<IActionResult> Put(int id, [FromBody] BrandInputModel newBrand)
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
        public async Task<IActionResult> Delete(int id)
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
