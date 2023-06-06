using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class UserInfoInput
{
    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(50, ErrorMessage = "The Name field must not exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Surname field is required.")]
    [StringLength(50, ErrorMessage = "The Surname field must not exceed 50 characters.")]
    public string Surname { get; set; }
}
