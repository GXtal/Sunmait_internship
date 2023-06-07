﻿namespace Web.Models.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public int BrandId { get; set; }

    public string BrandName { get; set;}
}
