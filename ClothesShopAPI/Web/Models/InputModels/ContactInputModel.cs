using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class ContactInputModel
{
    [Required(ErrorMessage = "The PhoneNumber field is required.")]
    [StringLength(20, ErrorMessage = "The PhoneNumber field must not exceed 20 characters.")]
    public string PhoneNumber { get; set; }
}
