using System.ComponentModel.DataAnnotations;

namespace HjortApi.Models;

public class DrinkUpdateReqModel
{
    [Required(ErrorMessage = "Drink name is required.")]
    [MinLength(1, ErrorMessage = "The drink name has to be at least 1 character long.")]
    [MaxLength(200, ErrorMessage = "The drink name cannot exceed 200 characters.")]
    public string Name { get; set; }
}
