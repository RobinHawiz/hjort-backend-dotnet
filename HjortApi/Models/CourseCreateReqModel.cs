using System.ComponentModel.DataAnnotations;

namespace HjortApi.Models;

public class CourseCreateReqModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Course menu id is required and has to be a positive number.")]
    public int CourseMenuId { get; set; }

    [Required(ErrorMessage = "Course name is required.")]
    [MinLength(1, ErrorMessage = "The course name has to be at least 1 character long.")]
    [MaxLength(200, ErrorMessage = "The course name cannot exceed 200 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Course type is required.")]
    [RegularExpression("starter|main|dessert", ErrorMessage = "Invalid course type. Expected one of: starter, main, or dessert.")]
    public string Type { get; set; }
}
