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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SectionViewModel>))]
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

    // POST api/Sections
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSection([FromBody] SectionInputModel newSection)
    {
        await _sectionService.AddSection(newSection.Name);
        return new OkResult();
    }

    // PUT api/Sections/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSection([FromRoute] int id, [FromBody] SectionInputModel newSection)
    {
        await _sectionService.RenameSection(id, newSection.Name);
        return new OkResult();
    }

    // DELETE api/Sections/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveSection([FromRoute] int id)
    {
        await _sectionService.RemoveSection(id);
        return new OkResult();
    }
}
