using System.Collections.Generic;

namespace ToDoList.Models
{
  public class Category
  {
    public Category()
    {
      this.Items = new HashSet<Item>();
    }
    // A HashSet is an unordered collection of unique elements. We create a HashSet of Items in the constructor to help avoid exceptions when no records exist in the "many" side of the relationship.

    public int CategoryId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Item> Items { get; set; }
    // Declaring Items as an instance of ICollection, a generic interface built into the .NET Framework.
    // An interface is a collection of method signatures bundled together. Interfaces are often likened to "contracts" the developer "signs" because whenever a class extends an interface, it must include every method outlined in the interface.
    // ICollection is specifically a generic interface, which means it contains a bundle of different menthods meant to work on a generic collection.
    // We use ICollection specifically because EF requires it. ICollection outlines methods for querying and changing data, which is functionality EF needs to work the "ORM magic" preventing us from having to manually interact with our database like we do when using SQL directly. 
  }
}