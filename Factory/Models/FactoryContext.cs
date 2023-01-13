using Microsoft.EntityFrameworkCore;
// Identity
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Factory.Models
{
  public class FactoryContext : IdentityDbContext<FactoryManager>
  {
    public DbSet<Engineer> Engineers { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<EngineerMachine> EngineerMachines { get; set; }
    public FactoryContext(DbContextOptions options) : base(options) { }
  }
}