using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Treats.Models
{
  public class Treat
  {
    [Required(ErrorMessage = "Enter a treat name.")]
    public string Name { get; set; }
    public int TreatId { get; set; }

    public List<FlavorTreat> FlavorTreats { get; }
  }
}