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

    public ActionResult Details(int id)
    {
      var thisItem = _db.Items
          .Include(item => item.Categories) // loads the Categories property of each Item
          .ThenInclude(join => join.Category) // loads the associated Category of each CategoryItem
          .FirstOrDefault(item => item.ItemId == id); // specifies which item from the database we are working with
      return View(thisItem);
    }
    
    // public ActionResult Details(int id)
    // {
    //   Item thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
    //   // FirstOrDefault() uses a lambda. We can read this as: start by looking at db.Items (our items table), then find any items where the ItemId of an item is equal to the id we've passed into this method.
    //   return View(thisItem);
    // }

    // public ActionResult Create()
    // {
    //   ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
    //   return View();
    // }

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