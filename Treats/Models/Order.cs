namespace Treats.Models
{
  public class Order
  {
    public int OrderId { get; set; }
    public int FlavorTreatId { get; set; }
    public FlavorTreat FlavorTreat { get; set; }
    public ApplicationUser User { get; set; }
  }
}