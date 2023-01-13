using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
// Identity
using Microsoft.AspNetCore.Identity;
// Async Task Class
using System.Threading.Tasks;
// Claims Class
using System.Security.Claims;

namespace Factory.Controllers
{
  public class HomeController : Controller
  {
    private readonly FactoryContext _db;
    private readonly UserManager<FactoryManager> _userManager;

    public HomeController(UserManager<FactoryManager> userManager, FactoryContext db)
    {
      _db = db;
      _userManager = userManager;
    }
    [HttpGet("/")]
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      FactoryManager currentManager = await _userManager.FindByIdAsync(userId);
      if (currentManager != null)
      {
        List<Location> managerLocations = _db.Locations
          .Where(entry => entry.Manager.Id == currentManager.Id)
          .Include(entry => entry.Engineers)
          .Include(entry => entry.Machines)
          .ToList();
        return View(managerLocations);
      }
      return View();
    }
  }
}