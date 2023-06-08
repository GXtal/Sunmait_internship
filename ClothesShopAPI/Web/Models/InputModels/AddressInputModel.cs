using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels;

public class AddressInputModel
{
    [Required(ErrorMessage = "The FullAddress field is required.")]
    [StringLength(200, ErrorMessage = "The FullAddress field must not exceed 200 characters.")]
    public string FullAddress { get; set; }
}
