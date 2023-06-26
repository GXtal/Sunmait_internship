using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class ReviewInputModel
{
    [Required(ErrorMessage = "The Rating field is required.")]
    [Range(1, 5, ErrorMessage = "The Rating is between 1 and 5")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "The Comment field is required.")]
    [StringLength(200, ErrorMessage = "The Comment field must not exceed 200 characters.")]
    public string Comment { get; set; }

    [Required(ErrorMessage = "The ProductId field is required.")]
    public int ProductId { get; set; }
}
