using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
  public class ToDoListContext : DbContext
  {
    public virtual DbSet<Category> Categories { get; set; }
    // Declaring something as virtual tells our application that we reserve the right to override it. By default, all methods are NOT virtual, which means they can't be overridden.
    // The virtual modifier cannot be used with static, abstract, or private modifier. In the code above, Categories is public so it can be virtual.
    // Making a property virtual allows Entity Framework (EF) to use lazy loading. Lazy loading just means that EF will only retrieve information that our code explicitly requires. This is considered to be far more efficient.
    public DbSet<Item> Items { get; set; }

    public ToDoListContext(DbContextOptions options) : base(options) { }
  }
}