using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class AddressInputModel
{
    [Required(ErrorMessage = "The FullAddress field is required.")]
    [StringLength(15, ErrorMessage = "The FullAddress field must not exceed 15 characters.")]
    public string FullAddress { get; set; }
}
