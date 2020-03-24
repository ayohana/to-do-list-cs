using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // allow us to actually authorize users.
using Microsoft.AspNetCore.Identity; // so our controller can interact with users from the database.
using System.Threading.Tasks; // so we can call async methods.
using System.Security.Claims; // so we can use claim based authorization. A claim is a form of user identification.
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList.Controllers
{
  [Authorize] // allows access to ItemsController only if a user is logged in
  public class ItemsController : Controller
  {
    private readonly ToDoListContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public ItemsController(UserManager<ApplicationUser> userManager, ToDoListContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // this refers to the ItemsController itself. FindFirst() is a method that locates the first record that meets the provided criteria. This method takes ClaimTypes.NameIdentifier as an argument. This is why we need a using directive for System.Security.Claims. We specify ClaimTypes.NameIdentifier to locate the unique ID associated with the current account. NameIdentifier is a property that refers to an Entity's unique ID. Finally, we include the ? operator called an existential operator. It states that we should only call the method to the right of the ? if the method to the left of the ? doesn't return null.
      var currentUser = await _userManager.FindByIdAsync(userId); // We call the FindByIdAsync() method, which, as its name suggests, is a built-in Identity method used to find a user's account by their unique ID.
      var userItems = _db.Items.Where(entry => entry.User.Id == currentUser.Id).ToList(); // We use the Where() method, which is a LINQ method we can use to query a collection in a way that echoes the logic of SQL. We can use Where() to make many different kinds of queries, as the method accepts an expression to filter our results. In this case, we're simply asking Entity to find items in the database where the user id associated with the item is the same id as the id that belongs to the currentUser. This ensures users only see their own tasks in the view.
      return View(userItems);
    }

    // Many-to-many relationship code:
    // public ActionResult Index()
    // {
    //   return View(_db.Items.ToList());
    // }

    // One-to-many relationship code:
    //     public ActionResult Index()
    //     {
    //       List<Item> model = _db.Items.Include(items => items.Category).ToList();
    //       return View(model);
    //     }
    //     // Using eager loading here, meaning that all info related to an object should be loaded by using Include()
    //     // This basically states: for each Item in the database, include the Category it belongs to and then put all the Items into list
    //     // We use eager loading instead of lazy loading because each item has only one category so there is a minimal amount of additional loading happening when we get information about an item

    public ActionResult Details(int id)
    {
      var thisItem = _db.Items
          .Include(item => item.Categories) // loads the Categories property of each Item
          .ThenInclude(join => join.Category) // loads the associated Category of each CategoryItem
          .FirstOrDefault(item => item.ItemId == id); // specifies which item from the database we are working with
      return View(thisItem);
    }
    
    // One-to-many relationship code:
    // public ActionResult Details(int id)
    // {
    //   Item thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
    //   // FirstOrDefault() uses a lambda. We can read this as: start by looking at db.Items (our items table), then find any items where the ItemId of an item is equal to the id we've passed into this method.
    //   return View(thisItem);
    // }

    public ActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Item item, int CategoryId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      item.User = currentUser;
      _db.Items.Add(item);
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    // Many-to-many relationship code:
    // [HttpPost]
    // public ActionResult Create(Item item, int CategoryId)
    // {
    //   _db.Items.Add(item);
    //   if (CategoryId != 0) // we add this condition to handle cases where a CategoryId doesn't get passed in to the route (such as when there are no Categories)
    //   {
    //     _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
    //     // this creates the association between the newly created Item and a Category. Because the Item has been added and a new ItemId has been assigned, we can create a new CategoryItem join entity. This combines the ItemId with the CategoryId specified in the dropdown menu and passed in through our route's parameters.
    //   }
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }

    // One-to-many relationship code:
    // [HttpPost]
    // public ActionResult Create(Item item)
    // {
    //   _db.Items.Add(item);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }

    public ActionResult Edit(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult Edit(Item item, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
      }
      _db.Entry(item).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddCategory(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult AddCategory(Item item, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    // One-to-many relationship code:
    // [HttpPost]
    // public ActionResult Edit(Item item)
    // {
    //   _db.Entry(item).State = EntityState.Modified; // updates item's State property to EntityState.Modified so that Entity knows that the entry has been modified
    //   _db.SaveChanges(); // ask the database to save the changes
    //   return RedirectToAction("Index");
    // }

    public ActionResult Delete(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
      return View(thisItem);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
      _db.Items.Remove(thisItem);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteCategory(int joinId)
    {
      var joinEntry = _db.CategoryItem.FirstOrDefault(entry => entry.CategoryItemId == joinId);
      _db.CategoryItem.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    // We use the name joinId in the DeleteCategory() route instead of id becuase .NET automatically utilizes the value in the URL query if we name the variable id. For example, if we named the parameter id instead of joinId and the details URL was something like /Items/Details/6, then the value of id would be 6, which is the ItemId and not the CategoryItemId that we wanted from our Hidden() method.
    // The reason .NET uses this convention is due to our configuration in the Startup.cs file. 
  }
}