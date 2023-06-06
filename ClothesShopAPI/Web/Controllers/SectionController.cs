using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Sections")]
[ApiController]
public class SectionController : ControllerBase
{
    private readonly ISectionService _sectionService;

    public SectionController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }

    // GET: api/Sections
    [HttpGet]
    public async Task<IActionResult> GetSections()
    {
        var allSections = await _sectionService.GetSections();

        var result = new List<SectionViewModel>();
        foreach (var section in allSections)
        {
            result.Add(new SectionViewModel { Id = section.Id, Name = section.Name });
        }

        return new OkObjectResult(result);
    }

    // GET api/Sections/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSectionById([FromRoute] int id)
    {
        var section = await _sectionService.GetSection(id);
        var result = new SectionViewModel() { Id = section.Id, Name = section.Name };
        return new OkObjectResult(result);
    }

    // POST api/Sections
    [HttpPost]
    public async Task<IActionResult> AddSection([FromBody] SectionInputModel newSection)
    {
        await _sectionService.AddSection(newSection.Name);
        return new OkResult();
    }

    // PUT api/Sections/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSection([FromRoute] int id, [FromBody] SectionInputModel newSection)
    {
        await _sectionService.RenameSection(id, newSection.Name);
        return new OkResult();
    }

    // DELETE api/Sections/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveSection([FromRoute] int id)
    {
        await _sectionService.RemoveSection(id);
        return new OkResult();
    }
}
