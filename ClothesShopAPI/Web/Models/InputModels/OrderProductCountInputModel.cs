using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class OrderProductCountInputModel
{
    [Required(ErrorMessage = "The Count field is required.")]
    [Range(1, 10, ErrorMessage = "The Count field must be positive and not greater then 10")]
    public int Count { get; set; }
}
