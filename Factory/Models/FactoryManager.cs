using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Factory.Models
{
  public class FactoryManager : IdentityUser
  {
    [Required]
    [RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Invalid Name")]
    public string FirstName {get;set;}
    [Required]
    [RegularExpression("^[a-zA-Z .]+$", ErrorMessage = "Invalid Name")]
    public string LastName {get;set;}

    // public static async string GetFirstName(this IIdentity identity )
    // {
    //   if (identity == null)
    //     return null;
    //   var firstName = (identity as ClaimsIdentity).FirstOrNull("FirstName");
    //   return firstName;
    // }
    // internal static string FirstOrNull(this ClaimsIdentity identity, string claimsType)
    // {
    //   var val = identity.FindFirst(claimsType);
    //   return val == null ? null : val.Value;
    // }
  }
}