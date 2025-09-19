using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Models;

public class CourseModel
{
    public int Id { get; set; }
    public int CourseMenuId { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
}
