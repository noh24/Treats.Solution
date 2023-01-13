using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Factory.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController(FactoryContext db)
    {
      _db = db;
    }

    // Routes
    public ActionResult Index()
    {
      List<Machine> model = _db.Machines
        .Include(machine => machine.Location)
        .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Machine machine)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
        return View(machine);
      }
      else
      {
        if (machine.LocationId == 0)
        {
          return RedirectToAction("Create");
        }
        _db.Machines.Add(machine);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    public ActionResult Details(int id)
    {
      Machine thisMachine = _db.Machines
        .Include(machine => machine.Location)
        .Include(machine => machine.EngineerMachines)
        .ThenInclude(engineerMachine => engineerMachine.Engineer)
        .FirstOrDefault(machine => machine.MachineId == id);
      return View(thisMachine);
    }

    public ActionResult Edit(int id)
    {
      Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
      return View(thisMachine);
    }

    [HttpPost]
    public ActionResult Edit(Machine machine)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.LocationId = new SelectList(_db.Locations, "LocationId", "Name");
        return View(machine);
      }
      _db.Machines.Update(machine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
    [HttpPost]
    public ActionResult Delete(int id)
    {
      Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      _db.Machines.Remove(thisMachine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddEngineer(int id)
    {
      Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View(thisMachine);
    }

    [HttpPost]
    public ActionResult AddEngineer(Machine machine, int engineerId)
    {
#nullable enable
      EngineerMachine? engineerMachineEntity = _db.EngineerMachines.FirstOrDefault(engineerMachine => (engineerMachine.MachineId == machine.MachineId && engineerMachine.EngineerId == engineerId));
#nullable disable
      if (engineerMachineEntity == null && engineerId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() { EngineerId = engineerId, MachineId = machine.MachineId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = machine.MachineId });
    }
    
    [HttpPost]
    public ActionResult DeleteMachine(int id)
    {
      EngineerMachine thisEngineerMachine = _db.EngineerMachines.FirstOrDefault(entry => entry.EngineerMachineId == id);
      _db.EngineerMachines.Remove(thisEngineerMachine);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisEngineerMachine.MachineId });
    }

    [HttpPost]
    public ActionResult MachineStatus(int id, string source)
    {
      EngineerMachine thisEngineerMachine = _db.EngineerMachines.FirstOrDefault(entry => entry.EngineerMachineId == id);
      if (source == "Engineers")
      {
        Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == thisEngineerMachine.MachineId);
        if (thisMachine.Status == 0)
        {
          thisMachine.Status = 1;
        }
        else if (thisMachine.Status == 1)
        {
          thisMachine.Status = 2;
        }
        else if (thisMachine.Status == 2)
        {
          thisMachine.Status = 0;
        }
        _db.Machines.Update(thisMachine);
        _db.SaveChanges();
        return RedirectToAction("Details", "Engineers", new { id = thisEngineerMachine.EngineerId });
      }
      else
      {
        Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == thisEngineerMachine.EngineerId);
        if (thisEngineer.Status == false)
        {
          thisEngineer.Status = true;
        }
        else
        {
          thisEngineer.Status = false;
        }
        _db.Engineers.Update(thisEngineer);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = thisEngineerMachine.MachineId });
      }
    }
  }
}