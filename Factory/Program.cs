using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Factory.Models;
// Identity
using Microsoft.AspNetCore.Identity;

namespace Factory
{
  class Program
  {
    static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      // add EF Core as a service to our To Do List app, specify ToDoListContext as the type
      builder.Services.AddDbContext<FactoryContext>(
                        dbContextOptions => dbContextOptions
                          .UseMySql(
                            builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                          )
                        )
                      );
      // Identity
      builder.Services.AddIdentity<FactoryManager, IdentityRole>()
        .AddEntityFrameworkStores<FactoryContext>()
        .AddDefaultTokenProviders();
      // Password Customize
      builder.Services.Configure<IdentityOptions>(options =>
      {
        // Default Password settings.
        // options.Password.RequireDigit = true;
        // options.Password.RequireLowercase = true;
        // options.Password.RequireNonAlphanumeric = true;
        // options.Password.RequireUppercase = true;
        // options.Password.RequiredLength = 6;
        // options.Password.RequiredUniqueChars = 1;
        // Customize Password settings
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
      });
      WebApplication app = builder.Build();

      // app.UseDeveloperExceptionPage();
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      // Identity
      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
      );

      app.Run();
    }
  }
}