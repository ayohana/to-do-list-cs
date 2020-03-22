using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

// Design Time = the time when we are actually writing and designing our code. Running migrations is something we do when we are designing our application, not when we are actually running our application.

namespace ToDoList.Models
{
  // A factory for creating derived DbContext instances. Implementing this interface will enable design-time services such as Migrations.
  public class ToDoListContextFactory : IDesignTimeDbContextFactory<ToDoListContext>
  {

    ToDoListContext IDesignTimeDbContextFactory<ToDoListContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var builder = new DbContextOptionsBuilder<ToDoListContext>();
      var connectionString = configuration.GetConnectionString("DefaultConnection");

      builder.UseMySql(connectionString);

      return new ToDoListContext(builder.Options);
    }
  }
}