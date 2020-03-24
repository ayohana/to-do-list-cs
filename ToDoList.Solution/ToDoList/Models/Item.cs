using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
  public class Item
  {
    public Item()
    {
      this.Categories = new HashSet<CategoryItem>();
    }
    public int ItemId { get; set; }
    public string Description { get; set; }
    public virtual ApplicationUser User { get; set; }
    // The User property is declared virtual to allow Entity to lazy load its contents, improving our application's efficiency.
    public ICollection<CategoryItem> Categories { get;}
  }
}