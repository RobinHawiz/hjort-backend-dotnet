using System.ComponentModel.DataAnnotations;

namespace HjortApi.Models;

public class DrinkCreateReqModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Drink menu id is required and has to be a positive number.")]
    public int DrinkMenuId { get; set; }

    [Required(ErrorMessage = "Drink name is required.")]
    [MinLength(1, ErrorMessage = "The drink name has to be at least 1 character long.")]
    [MaxLength(200, ErrorMessage = "The drink name cannot exceed 200 characters.")]
    public string Name { get; set; } = default!;
}
