using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Treats.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;


namespace Treats.Controllers
{
  public class OrdersController : Controller
  {
    private readonly TreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrdersController(UserManager<ApplicationUser> userManager, TreatsContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    public async Task<ActionResult> Index()
    {
      ViewBag.Title = "Cart";
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Order> orders = _db.Orders
        .Where(entry => entry.User.Id == currentUser.Id)
        .Include(entry => entry.FlavorTreat)
        .ToList();
      return View(orders);
    }
    public ActionResult Create()
    {
      ViewBag.Title = "Add To Cart";
      ViewBag.FlavorTreatId = new SelectList(_db.FlavorTreats, "FlavorTreatId", "Name");
      return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(Order order)
    {
      if (ModelState.IsValid)
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        order.User = currentUser;
        _db.Orders.Add(order);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      ViewBag.FlavorTreatId = new SelectList(_db.FlavorTreats, "FlavorTreatId", "Name");
      return RedirectToAction("Create");
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Edit Order";
      Order thisOrder = _db.Orders.FirstOrDefault(entry => entry.OrderId == id);
      ViewBag.FlavorTreatId = new SelectList(_db.FlavorTreats, "FlavorTreatId", "Name");
      return View(thisOrder);
    }
    [HttpPost]
    public ActionResult Edit(Order order)
    {
      _db.Orders.Update(order);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult Delete(int id)
    {
      Order thisOrder = _db.Orders.FirstOrDefault(entry => entry.OrderId == id);
      _db.Orders.Remove(thisOrder);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}