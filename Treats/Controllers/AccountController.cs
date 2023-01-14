using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Treats.Models;
using System.Threading.Tasks;
using Treats.ViewModels;
using System.Security.Claims;

namespace Treats.Controllers
{
  public class AccountController : Controller
  {
    private readonly TreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TreatsContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }
    public async Task<ActionResult> Index()
    {
      ViewBag.Title = "Account";
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      return View(currentUser);
    }
    public async Task<ActionResult> IndexConfirm()
    {
      ViewBag.Title = "Registration Successful";
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      return View(currentUser);
    }

    public IActionResult Register()
    {
      ViewBag.Title = "Sign up";
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      if (ModelState.IsValid)
      {
        ApplicationUser user = new ApplicationUser { UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("IndexConfirm");
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
      ViewBag.Title = "Sign in";
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
          ModelState.AddModelError("", "There is something wrong with your email or password. Please try again.");
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

    public IActionResult AccessDenied()
    {
      return RedirectToAction("Login");
    }
  }
}