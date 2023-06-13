using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models.InputModels;
using Web.Models.ViewModels;

namespace Web.Controllers;
[Route("api/Images")]
[ApiController]
public class ImageController : ControllerBase
{
    private const string MimeType = "image/png";

    private readonly IImageService _imageService;
    private readonly IWebHostEnvironment _environment;

    public ImageController(IImageService imageService, IWebHostEnvironment environment)
    {
        _imageService = imageService;
        _environment = environment;
    }

    [HttpPost("Products/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddImage([FromRoute] int productId,[FromForm] ImageInputModel newImage)
    {
        if (newImage.formFile.Length > 0)
        {
            var type = newImage.formFile.ContentType;

            byte[] content = new byte[newImage.formFile.Length];
            var stream = new MemoryStream(content);
            newImage.formFile.CopyTo(stream);

            await _imageService.AddImage(content, productId);
        }

        return new OkResult();
    }

    [HttpGet("Products/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<int>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImageIdsByProduct([FromRoute] int productId)
    {
        var imageIds = await _imageService.GetImageIdsByProduct(productId);

        var result = new List<ImageIdViewModel>();

        foreach (var imageId in imageIds)
        {
            result.Add(new ImageIdViewModel { Id = imageId });
        }

        return new OkObjectResult(imageIds);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(byte[]))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImageById([FromRoute] int id)
    {
        var image = await _imageService.GetImageById(id);

        var result = image.Content;

        return File(result, MimeType);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveImage([FromRoute] int id)
    {
        await _imageService.RemoveImage(id);
        return new OkResult();
    }
}
