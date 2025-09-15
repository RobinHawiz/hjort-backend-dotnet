using System.ComponentModel.DataAnnotations;

namespace HjortApi.Models;

public class CourseMenuReqModel
{
    [Required(ErrorMessage = "Course menu title is required.")]
    [MinLength(1, ErrorMessage = "The course menu title has to be at least 1 character long.")]
    [MaxLength(50, ErrorMessage = "The course menu title cannot exceed 50 characters.")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Price total is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Price total is required and has to be a positive number.")]
    public int PriceTot { get; set; }
}
