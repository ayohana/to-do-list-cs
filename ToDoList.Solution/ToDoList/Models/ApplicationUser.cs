using Microsoft.AspNetCore.Identity;
// Identity comes with a class to represent users called IdentityUser. As we can see in the IdentityUser documentation entry, this class contains a number of properties that can be used Emails, UserNames, and many other properties including recording the number of failed login attempts.

namespace ToDoList.Models
{
  public class ApplicationUser : IdentityUser
  {

  }
}