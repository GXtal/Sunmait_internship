using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class ContactInputModel
{
    [Required(ErrorMessage = "The PhoneNumber field is required.")]
    [StringLength(15, ErrorMessage = "The PhoneNumber field must not exceed 15 characters.")]
    public string PhoneNumber { get; set; }
}
