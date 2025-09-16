using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models;
public class DrinkModel
{
    public int Id { get; set; }
    public int DrinkMenuId { get; set; }
    public string Name { get; set; }
}
