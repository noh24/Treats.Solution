using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
  public class Location
  {
    [Required(ErrorMessage = "Name must be entered!")]
    public string Name { get; set; }
    public int LocationId { get; set; }
    public List<Engineer> Engineers { get; set; }
    public List<Machine> Machines { get; set; }
    public FactoryManager Manager { get; set; }
  }
}