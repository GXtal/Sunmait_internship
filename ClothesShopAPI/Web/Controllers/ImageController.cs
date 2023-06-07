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
    private readonly IImageService _imageService;
    private readonly IWebHostEnvironment _environment;

    public ImageController(IImageService imageService, IWebHostEnvironment environment)
    {
        _imageService = imageService;
        _environment = environment;
    }

    [HttpPost]
    public async Task<IActionResult> AddImage([FromForm] ImageInputModel newImage)
    {
        if (newImage.formFile.Length > 0)
        {
            if (!Directory.Exists(_environment.WebRootPath + "\\Images"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "\\Images\\");
            }

            var filePath = _environment.WebRootPath + "\\Images\\" + newImage.formFile.FileName;
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                newImage.formFile.CopyTo(fileStream);
                fileStream.Flush();
            }

            await _imageService.AddImage(filePath, newImage.ProductId);
        }

        return new OkResult();
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetImagesByProduct([FromRoute] int productId)
    {
        var images = await _imageService.GetImagesByProduct(productId);

        var result = new List<ImageViewModel>();
        foreach (var image in images)
        {
            result.Add(new ImageViewModel() { Id = image.Id, Path = image.Path });
        }

        return new OkObjectResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveImage([FromRoute] int id)
    {
        await _imageService.RemoveImage(id);
        return new OkResult();
    }
}
