﻿namespace Web.Models.InputModels;

public class ImageInputModel
{
    public IFormFile formFile { get; set; }

    public int ProductId { get; set; }
}
