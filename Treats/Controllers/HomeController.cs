using Microsoft.AspNetCore.Mvc;
using Treats.Models;
using System.Collections.Generic;
using System.Linq;

namespace Treats.Controllers
{
  public class HomeController : Controller
  {
    private readonly TreatsContext _db;

    public HomeController(TreatsContext db)
    {
      _db = db;
    }
    [HttpGet("/")]
    public ActionResult Index()
    {
      ViewBag.Title = "Home";
      Flavor[] flavors = _db.Flavors.OrderBy(entry => entry.Description).ToArray();
      Treat[] treats = _db.Treats.OrderBy(entry => entry.Name).ToArray();
      Dictionary<string, object[]> model = new Dictionary<string, object[]>();
      model.Add("flavors", flavors);
      model.Add("treats", treats);
      return View(model);
    }
  }
}