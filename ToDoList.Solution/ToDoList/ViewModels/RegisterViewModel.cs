using System.ComponentModel.DataAnnotations;

// When we deal with data that only shows up in the view, we can use a ViewModel instead of a Model. This allows us to specify which fields we want to collect from our view. Since we don't need to collect information for all the properties built into ApplicationUser, we'll create a ViewModel that only contains the properties we need.
// While this ViewModel may look a different from other classes at first, it's really just a grouping of properties and data annotations. Now we have a data model we can use for user registration.

namespace ToDoList.ViewModels
{
  public class RegisterViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}

// Note that Identity's default setting for a password is at least six characters, a capital letter, a digit, and a special character.