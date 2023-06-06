using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewModels;

namespace Web.Controllers;

[Route("api/Status")]
[ApiController]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    // GET: api/Statuss
    [HttpGet]
    public async Task<IActionResult> GetStatuss()
    {
        var allStatuses = await _statusService.GetStatuses();

        var result = new List<StatusViewModel>();
        foreach (var status in allStatuses)
        {
            result.Add(new StatusViewModel { Id = status.Id, Name = status.Name });
        }

        return new OkObjectResult(result);
    }

    // GET api/Statuss/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStatusById([FromRoute] int id)
    {
        var status = await _statusService.GetStatus(id);
        var result = new StatusViewModel() { Id = status.Id, Name = status.Name };
        return new OkObjectResult(result);
    }
}
