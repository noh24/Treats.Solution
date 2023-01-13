using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Factory.Models
{
  public class Machine
  {
    [Required(ErrorMessage = "Machine name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "What does the machine do?")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Enter a date")]
    [CannotBeFuture]
    public Nullable<DateTime> InstallmentDate { get; set; }
    public int MachineId { get; set; }
    public int Status { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
    public List<EngineerMachine> EngineerMachines { get; }

  }
  public class CannotBeFutureAttribute : RangeAttribute
  {
    public CannotBeFutureAttribute()
      : base(typeof(DateTime),
              DateTime.MinValue.ToShortDateString(),
              DateTime.Now.ToShortDateString())
    { }
    public override string FormatErrorMessage(string name)
    {
      return $"You cannot add a machine with a future installment date.";
    }
  }
}