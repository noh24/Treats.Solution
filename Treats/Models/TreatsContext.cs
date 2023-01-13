using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Treats.Models
{
  public class TreatsContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Flavor> Flavors { get; set; }
    public DbSet<Treat> Treats { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<FlavorTreat> FlavorTreats { get; set; }
    public TreatsContext(DbContextOptions options) : base(options) { }
  }
}