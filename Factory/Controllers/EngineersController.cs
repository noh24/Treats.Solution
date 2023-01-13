using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Factory.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Factory.Controllers
{
  public class EngineersController : Controller
  {
    private readonly FactoryContext _db;

    public EngineersController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Engineer> model = _db.Engineers
        .Include(engineer => engineer.Location)
        .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
      return View();
    }
    [HttpPost]
    public ActionResult Create(Engineer engineer)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
        return View(engineer);
      }
      else
      {
        if (engineer.LocationId == 0)
        {
          return RedirectToAction("Create");
        }
        _db.Engineers.Add(engineer);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Engineer thisEngineer = _db.Engineers
        .Include(engineer => engineer.Location)
        .Include(engineer => engineer.EngineerMachines)
        .ThenInclude(engineerMachines => engineerMachines.Machine)
        .FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    public ActionResult Edit(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult Edit(Engineer engineer)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
        return View(engineer);
      }
      _db.Engineers.Update(engineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
    [HttpPost]
    public ActionResult Delete(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      _db.Engineers.Remove(thisEngineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddMachine(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult AddMachine(Engineer engineer, int machineId)
    {
#nullable enable
      EngineerMachine? engineerMachineEntity = _db.EngineerMachines.FirstOrDefault(engineerMachine => (engineerMachine.MachineId == machineId && engineerMachine.EngineerId == engineer.EngineerId));
#nullable disable
      if (engineerMachineEntity == null && machineId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() { MachineId = machineId, EngineerId = engineer.EngineerId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = engineer.EngineerId });
    }

    [HttpPost]
    public ActionResult DeleteMachine(int id)
    {
      EngineerMachine thisEngineerMachine = _db.EngineerMachines.FirstOrDefault(entry => entry.EngineerMachineId == id);
      _db.EngineerMachines.Remove(thisEngineerMachine);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisEngineerMachine.EngineerId });
    }
  }
}