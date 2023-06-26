using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class LoginInputModel
{
    [Required(ErrorMessage = "The Email field is required.")]
    [StringLength(50, ErrorMessage = "The Email field must not exceed 50 characters.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The Password field is required.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Password field length is between 3 and 20")]
    public string Password { get; set; }
}
