using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Treats.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Treats.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly TreatsContext _db;

    public FlavorsController(TreatsContext db)
    {
      _db = db;
    }
    [AllowAnonymous]
    public ActionResult Index()
    {
      ViewBag.Title = "Flavors";
      List<Flavor> flavors = _db.Flavors.OrderBy(entry => entry.Description).ToList();
      return View(flavors);
    }
    [Authorize(Roles = "Manager")]
    public ActionResult Create()
    {
      ViewBag.Title = "Create Flavors";
      return View();
    }
    [HttpPost]
    public ActionResult Create(Flavor flavor)
    {
      if (!ModelState.IsValid)
      {
        return View(flavor);
      }
      else
      {
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Flavor thisFlavor = _db.Flavors
        .Include(flavor => flavor.FlavorTreats)
        .ThenInclude(flavorTreats => flavorTreats.Treat)
        .FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.Title = $"{thisFlavor.Description}";
      return View(thisFlavor);
    }

    public ActionResult Edit(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.Title = $"Edit {thisFlavor.Description}";
      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult Edit(Flavor flavor)
    {
      if (!ModelState.IsValid)
      {
        return View(flavor);
      }
      _db.Flavors.Update(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
    [HttpPost]
    public ActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTreat(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.Title = $"Add Treat to {thisFlavor.Description}";
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult AddTreat(Flavor flavor, int treatId)
    {
#nullable enable
      FlavorTreat? flavorTreatEntity = _db.FlavorTreats.FirstOrDefault(flavorTreat => (flavorTreat.TreatId == treatId && flavorTreat.FlavorId == flavor.FlavorId));
#nullable disable
      if (flavorTreatEntity == null && treatId != 0)
      {
        Treat thisTreat = _db.Treats.FirstOrDefault(entry => entry.TreatId == treatId);
        Flavor thisFlavor = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == flavor.FlavorId);
        _db.FlavorTreats.Add(new FlavorTreat() { TreatId = treatId, FlavorId = flavor.FlavorId, Name = $"{thisFlavor.Description}" + " " + $"{thisTreat.Name}" });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = flavor.FlavorId });
    }

    [HttpPost]
    public ActionResult DeleteTreat(int id, int routeId)
    {
      FlavorTreat thisFlavorTreat = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == id);
      _db.FlavorTreats.Remove(thisFlavorTreat);
      _db.SaveChanges();
      if (routeId == 0)
      {
        return RedirectToAction("Details", new { id = thisFlavorTreat.FlavorId });
      }
      else 
      {
        return RedirectToAction("Details", "Treats", new { id = thisFlavorTreat.TreatId });
      }
    }
  }
}