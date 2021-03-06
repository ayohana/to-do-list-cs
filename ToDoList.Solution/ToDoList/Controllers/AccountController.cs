using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ToDoList.Models;
using System.Threading.Tasks; // allows us to use asynchronous Tasks so we can use async and await to register new users
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
  public class AccountController : Controller
  {
    private readonly ToDoListContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    // Dependency injection is the act of providing a helpful tool (known as a service) to part of an application that needs it before it actually needs it. This ensures that the application doesn't need to worry about locating, loading, finding, or creating that service on its own.
    // In our case, we're injecting the Identity's UserManager and SignInManager services into the AccountController constructor so that our controller will have access to these services as needed.

    public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ToDoListContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View();
    }

    public IActionResult Register()
    {
      return View();
    }

    // Remember, the built-in Task class represents asynchronous actions that haven't been completed yet.
    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model)
    {
      var user = new ApplicationUser { UserName = model.Email };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }
    //Note that CreateAsync() takes two arguments:
    // 1. An ApplicationUser with user information;
    // 2. A password that will be encrypted when the user is added to the database.

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }
    //Remember that we've injected a SignInManager service, which is being referenced in the signInManager variable. The SignInManager class includes the PasswordSignInAsync() method, which has a self-explanatory name: it's an async method that allows users to sign in with a password.
    // PasswordSignInAsync() takes four parameters: userName, password, isPersistent and lockoutOnFailure. For now we're only handling username and password, so we set explicit boolean values for isPersistent and lockoutOnFailure.

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }

  }
}