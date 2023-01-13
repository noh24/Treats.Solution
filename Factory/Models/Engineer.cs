using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
  public class Engineer
  {
    [Required(ErrorMessage = "Engineer name must be entered!")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Add Engineer's Specialty Machine")]
    public string SpecialtyMachine { get; set; }
    [FromNow]
    [Required(ErrorMessage = "Enter a date")]
    public Nullable<DateTime> LicenseRenewalDate { get; set; }
    public int EngineerId { get; set; }
    public bool Status { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
    public List<EngineerMachine> EngineerMachines { get; }
  }
  public class FromNowAttribute : RangeAttribute
  {
    public FromNowAttribute()
      : base(typeof(DateTime),
              DateTime.Now.ToShortDateString(),
              DateTime.MaxValue.ToShortDateString())
    { }
    public override string FormatErrorMessage(string name)
    {
      return $"Enter a license renewal date isn't already expired.";
    }
  }
}