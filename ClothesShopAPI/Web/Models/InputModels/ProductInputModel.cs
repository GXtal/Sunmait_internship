using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class ProductInputModel
{
    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(50, ErrorMessage = "The Name field must not exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Price field is required.")]
    [Range(0.01, 9999.99, ErrorMessage = "The Price field must be real")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The Description field is required.")]
    [StringLength(200, ErrorMessage = "The Description field must not exceed 200 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "The Quantity field is required.")]
    [Range(0, 1000, ErrorMessage = "The Quantity field must be real")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "The CategoryId field is required.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "The BrandId field is required.")]
    public int BrandId { get; set; }
}
