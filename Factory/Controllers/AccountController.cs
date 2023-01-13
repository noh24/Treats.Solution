using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Factory.Models;
using System.Threading.Tasks;
using Factory.ViewModels;
using System.Security.Claims;

namespace Factory.Controllers
{
  public class AccountController : Controller
  {
    private readonly FactoryContext _db;
    private readonly UserManager<FactoryManager> _userManager;
    private readonly SignInManager<FactoryManager> _signInManager;

    public AccountController(UserManager<FactoryManager> userManager, SignInManager<FactoryManager> signInManager, FactoryContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      FactoryManager currentUser = await _userManager.FindByIdAsync(userId);
      return View(currentUser);
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      if (ModelState.IsValid)
      {
        FactoryManager user = new FactoryManager { UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        // await _userManager.AddClaimAsync(user.Id, new Claim("FirstName", user.FirstName));
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
          return View(model);
        }
      }
      else
      {
        return View(model);
      }
    }

    public ActionResult Login()
    {
      return View();
    }
    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      if (ModelState.IsValid)
      {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ModelState.AddModelError("", "There is something wrong with your email or username. Please try again.");
          return View(model);
        }
      }
      else
      {
        return View(model);
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Login");
    }
  }
}