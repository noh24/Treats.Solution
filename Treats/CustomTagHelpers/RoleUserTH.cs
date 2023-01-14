using Treats.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Identity.CustomTagHelpers
{
  [HtmlTargetElement("td", Attributes = "i-role")]
  public class RoleUsersTH : TagHelper
  {
    private UserManager<ApplicationUser> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    public RoleUsersTH(UserManager<ApplicationUser> usermgr, RoleManager<IdentityRole> rolemgr)
    {
      _userManager = usermgr;
      _roleManager = rolemgr;
    }

    [HtmlAttributeName("i-role")]
    public string Role { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
      List<string> names = new List<string>();
      IdentityRole role = await _roleManager.FindByIdAsync(Role);
      if (role != null)
      {
        foreach (var user in _userManager.Users)
        {
          if (user != null && await _userManager.IsInRoleAsync(user, role.Name))
            names.Add(user.UserName);
        }
      }
      output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
    }
  }
}