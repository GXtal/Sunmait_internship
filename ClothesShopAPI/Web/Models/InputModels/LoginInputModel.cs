using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class LoginInputModel
{
    [Required(ErrorMessage = "The Email field is required.")]
    [StringLength(50, ErrorMessage = "The Email field must not exceed 50 characters.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The PasswordHash field is required.")]
    [StringLength(64, MinimumLength = 64, ErrorMessage = "Hash field has 64 lenght")]
    public string PasswordHash { get; set; }
}
