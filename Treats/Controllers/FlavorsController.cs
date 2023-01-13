using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Treats.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Treats.Controllers
{
  public class FlavorsController : Controller
  {
    private readonly TreatsContext _db;

    public FlavorsController(TreatsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      ViewBag.Title = "Flavors";
      List<Flavor> flavors = _db.Flavors.ToList();
      return View(flavors);
    }

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
        _db.FlavorTreats.Add(new FlavorTreat() { TreatId = treatId, FlavorId = flavor.FlavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = flavor.FlavorId });
    }

    [HttpPost]
    public ActionResult DeleteTreat(int id)
    {
      FlavorTreat thisFlavorTreat = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == id);
      _db.FlavorTreats.Remove(thisFlavorTreat);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisFlavorTreat.FlavorId });
    }
  }
}