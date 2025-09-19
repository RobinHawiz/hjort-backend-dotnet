using System.ComponentModel.DataAnnotations;

namespace HjortApi.Models;

public class DrinkMenuReqModel
{
    [MaxLength(50, ErrorMessage = "The drink menu title cannot exceed 50 characters.")]
    public string Title { get; set; } = default!;

    [MaxLength(50, ErrorMessage = "The drink menu subtitle cannot exceed 50 characters.")]
    public string Subtitle { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "Price total is required and has to be a positive number.")]
    public int PriceTot { get; set; }
}
