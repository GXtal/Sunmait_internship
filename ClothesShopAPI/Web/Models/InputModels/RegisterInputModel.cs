using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class RegisterInputModel
{
    [Required(ErrorMessage = "The Email field is required.")]
    [StringLength(50, ErrorMessage = "The Email field must not exceed 50 characters.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The PassworHash field is required.")]
    [StringLength(64, MinimumLength = 64, ErrorMessage = "Hash field has 64 lenght")]
    public string PasswordHash { get; set; }

    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(50, ErrorMessage = "The Name field must not exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Surname field is required.")]
    [StringLength(50, ErrorMessage = "The Surname field must not exceed 50 characters.")]
    public string Surname { get; set; }
}
