using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Factory.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
// Authorization
using Microsoft.AspNetCore.Authorization;
// Identity
using Microsoft.AspNetCore.Identity;
// Async Task Class
using System.Threading.Tasks;
// Claim based Authorization
using System.Security.Claims;

namespace Factory.Controllers
{
  [Authorize]
  public class LocationsController : Controller
  {
    private readonly FactoryContext _db;
    private readonly UserManager<FactoryManager> _userManager;

    public LocationsController(UserManager<FactoryManager> userManager, FactoryContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      FactoryManager currentManager = await _userManager.FindByIdAsync(userId);
      List<Location> managerLocations = _db.Locations
        .Where(entry => entry.Manager.Id == currentManager.Id)
        .ToList();
      return View(managerLocations);
    }

    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(Location location)
    {
      if (ModelState.IsValid)
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        FactoryManager currentManager = await _userManager.FindByIdAsync(userId);
        location.Manager = currentManager;
        _db.Locations.Add(location);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      else
      {
        return View(location);
      }
    }

    public ActionResult Details(int id)
    {
      Location thisLocation = _db.Locations
        .Include(location => location.Engineers)
          .ThenInclude(engineer => engineer.EngineerMachines)
          .ThenInclude(engineerMachine => engineerMachine.Machine)
        .Include(location => location.Machines)
          .ThenInclude(machine => machine.EngineerMachines)
          .ThenInclude(engineerMachine => engineerMachine.Engineer)
        .FirstOrDefault(location => location.LocationId == id);
      return View(thisLocation);
    }

    public ActionResult Edit(int id)
    {
      Location thisLocation = _db.Locations.FirstOrDefault(location => location.LocationId == id);
      return View(thisLocation);
    }
    [HttpPost]
    public ActionResult Edit(Location location)
    {
      _db.Locations.Update(location);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = location.LocationId});
    }
    [HttpPost]
    public ActionResult Delete(int id)
    {
      Location thisLocation = _db.Locations.FirstOrDefault(location => location.LocationId == id);
      _db.Locations.Remove(thisLocation);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}