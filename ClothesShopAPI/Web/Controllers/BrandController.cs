using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Inputs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;

        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        // GET: api/<BrandsController>
        [HttpGet]
        public IActionResult Get()
        {
            var allBrands = _brandRepository.GetBrands();
            return new OkObjectResult(allBrands);
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var brand = _brandRepository.GetBrandById(id);

            if (brand == null)
            {
                return NotFound();
            }

            return new OkObjectResult(brand);
        }

        // POST api/<BrandsController>
        [HttpPost]
        public IActionResult Post([FromBody] BrandInput newBrand)
        {
            bool result = _brandRepository.InsertBrand(newBrand);

            if (!result)
            {
                return BadRequest();
            }

            return new OkResult();
        }

        // PUT api/<BrandsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] BrandInput newBrand)
        {
            bool result = _brandRepository.UpdateBrand(id, newBrand);

            if (!result)
            {
                return BadRequest();
            }

            return new OkResult();
        }

        // DELETE api/<BrandsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _brandRepository.DeleteBrand(id);

            if (!result)
            {
                return NotFound();
            }

            return new OkResult();
        }
    }
}
