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
  public class TreatsController : Controller
  {
    private readonly TreatsContext _db;

    public TreatsController(TreatsContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      ViewBag.Title = "Treats";
      List<Treat> model = _db.Treats.OrderBy(entry => entry.Name).ToList();
      return View(model);
    }

    [Authorize(Roles = "Manager, Admin")]
    public ActionResult Create()
    {
      ViewBag.Title = "Create Treats";
      return View();
    }
    [HttpPost]
    public ActionResult Create(Treat treat)
    {
      if (!ModelState.IsValid)
      {
        return View(treat);
      }
      else
      {
        _db.Treats.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
        .Include(treat => treat.FlavorTreats)
        .ThenInclude(flavorTreat => flavorTreat.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.Title = $"{thisTreat.Name}";
      return View(thisTreat);
    }

    [Authorize(Roles = "Manager, Admin")]
    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.Title = $"Edit {thisTreat.Name}";
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      if (!ModelState.IsValid)
      {
        return View(treat);
      }
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize(Roles = "Manager, Admin")]
    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Description");
      ViewBag.Title = $"Add Flavor to {thisTreat.Name}";
      return View(thisTreat);
    }
    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
#nullable enable
      FlavorTreat? flavorTreatEntity = _db.FlavorTreats.FirstOrDefault(flavorTreat => (flavorTreat.TreatId == treat.TreatId && flavorTreat.FlavorId == flavorId));
#nullable disable
      if (flavorTreatEntity == null && flavorId != 0)
      {
        Treat thisTreat = _db.Treats.FirstOrDefault(entry => entry.TreatId == treat.TreatId);
        Flavor thisFlavor = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == flavorId);
        _db.FlavorTreats.Add(new FlavorTreat() { FlavorId = flavorId, TreatId = treat.TreatId, Name = $"{thisFlavor.Description}" + " " + $"{thisTreat.Name}" });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }
  }
}