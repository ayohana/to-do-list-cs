using Microsoft.EntityFrameworkCore; // to access Entity State
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
    private readonly ToDoListContext _db;

    public ItemsController(ToDoListContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Items.ToList());
    }

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
    public ActionResult Create(Item item, int CategoryId)
    {
      _db.Items.Add(item);
      if (CategoryId != 0) // we add this condition to handle cases where a CategoryId doesn't get passed in to the route (such as when there are no Categories)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
        // this creates the association between the newly created Item and a Category. Because the Item has been added and a new ItemId has been assigned, we can create a new CategoryItem join entity. This combines the ItemId with the CategoryId specified in the dropdown menu and passed in through our route's parameters.
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    // One-to-many relationship code:
    // [HttpPost]
    // public ActionResult Create(Item item)
    // {
    //   _db.Items.Add(item);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }

    // public ActionResult Edit(int id)
    // {
    //   var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
    //   ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
    //   return View(thisItem);
    // }

    // [HttpPost]
    // public ActionResult Edit(Item item)
    // {
    //   _db.Entry(item).State = EntityState.Modified; // updates item's State property to EntityState.Modified so that Entity knows that the entry has been modified
    //   _db.SaveChanges(); // ask the database to save the changes
    //   return RedirectToAction("Index");
    // }

    // public ActionResult Delete(int id)
    // {
    //   var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
    //   return View(thisItem);
    // }

    // [HttpPost, ActionName("Delete")]
    // public ActionResult DeleteConfirmed(int id)
    // {
    //   var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
    //   _db.Items.Remove(thisItem);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }
  }
}