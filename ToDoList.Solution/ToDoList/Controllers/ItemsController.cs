using Microsoft.EntityFrameworkCore; // to access Entity State
using Microsoft.AspNetCore.Mvc;
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
      List<Item> model = _db.Items.ToList(); // ToList() will give us access to all our Items in List
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Item item)
    {
      _db.Items.Add(item);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Item thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
      // FirstOrDefault() uses a lambda. We can read this as: start by looking at db.Items (our items table), then find any items where the ItemId of an item is equal to the id we've passed into this method.
      return View(thisItem);
    }

    public ActionResult Edit(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult Edit(Item item)
    {
      _db.Entry(item).State = EntityState.Modified; // updates item's State property to EntityState.Modified so that Entity knows that the entry has been modified
      _db.SaveChanges(); // ask the database to save the changes
      return RedirectToAction("Index");
    }

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
  }
}