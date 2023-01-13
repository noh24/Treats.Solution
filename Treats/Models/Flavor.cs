using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Treats.Models
{
  public class Flavor
  {
    [Required(ErrorMessage = "Enter a flavor description.")]
    public string Description { get; set; }
    public int FlavorId { get; set; }
    public List<FlavorTreat> FlavorTreats { get; }
  }
}