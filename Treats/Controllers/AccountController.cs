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
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, TreatsContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _roleManager = roleManager;
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

    public async Task<IActionResult> Register()
    {
      ViewBag.Title = "Sign up";
      bool adminExist = await _roleManager.RoleExistsAsync("Admin");
      if (!adminExist)
      {
        IdentityRole role = new IdentityRole() { Name = "Admin" };
        await _roleManager.CreateAsync(role);
        ApplicationUser user = new ApplicationUser() { UserName = "admin@admin.com", Email = "admin@admin.com", FirstName = "admin", LastName = "admin"};
        string password = "a";
        IdentityResult userValid = await _userManager.CreateAsync(user, password);
        if (userValid.Succeeded)
        {
          var result = await _userManager.AddToRoleAsync(user, "Admin");
        }
      }
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