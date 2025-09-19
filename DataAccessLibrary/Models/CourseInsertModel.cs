using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models;

public class CourseInsertModel
{
    public int CourseMenuId { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;

}
