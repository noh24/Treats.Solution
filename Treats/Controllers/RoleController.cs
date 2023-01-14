using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Treats.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Treats.Controllers
{
  public class RoleController : Controller
  {
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<ApplicationUser> _userManager;
    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
      _roleManager = roleManager;
      _userManager = userManager;
    }

    public ViewResult Index()
    {
      return View(_roleManager.Roles);
    }
    private void Errors(IdentityResult result)
    {
      foreach (IdentityError error in result.Errors)
        ModelState.AddModelError("", error.Description);
    }
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<ActionResult> Create([Required] string name)
    {
      if (ModelState.IsValid)
      {
        IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
        if (result.Succeeded)
          return RedirectToAction("Index");
        else
          Errors(result);
      }
      return View(name);
    }
    [HttpPost]
    public async Task<ActionResult> Delete(string id)
    {
      IdentityRole role = await _roleManager.FindByIdAsync(id);
      if (role != null)
      {
        IdentityResult result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
          return RedirectToAction("Index");
        else
          Errors(result);
      }
      else
        ModelState.AddModelError("", "No role found");
      return View("Index", _roleManager.Roles);
    }
    public async Task<IActionResult> Edit(string id)
    {
      IdentityRole role = await _roleManager.FindByIdAsync(id);
      IList<ApplicationUser> members = await _userManager.GetUsersInRoleAsync(role.Name);
      IList<ApplicationUser> nonMembers = await _userManager.GetUsersInRoleAsync(role.Name);
      // List<ApplicationUser> members = new List<ApplicationUser>();
      // List<ApplicationUser> nonMembers = new List<ApplicationUser>();
      // if (role != null)
      // {
        foreach (ApplicationUser user in _userManager.Users)
        {
          if (!await _userManager.IsInRoleAsync(user, role.Name))
          {
            nonMembers.Add(user);
          }
      //   }
      }
      RoleEdit newRoleEdit = new RoleEdit { Role = role, Members = members, NonMembers = nonMembers };
      return View(newRoleEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoleModification model)
    {
      IdentityResult result;
      if (ModelState.IsValid)
      {
        foreach (string userId in model.AddIds ?? new string[] { })
        {
          ApplicationUser user = await _userManager.FindByIdAsync(userId);
          if (user != null)
          {
            result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
              Errors(result);
          }
        }
        foreach (string userId in model.DeleteIds ?? new string[] { })
        {
          ApplicationUser user = await _userManager.FindByIdAsync(userId);
          if (user != null)
          {
            result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
              Errors(result);
          }
        }
      }

      if (ModelState.IsValid)
        return RedirectToAction(nameof(Index));
      else
        return await Edit(model.RoleId);
    }
  }
}