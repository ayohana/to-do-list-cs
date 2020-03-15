using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Item
  {
    public string Description { get; set; }
    public int Id { get; }

    public Item (string description)
    {
      Description = description;
    }

    public Item(string description, int id)
    {
      Description = description;
      Id = id;
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool descriptionEquality = (this.Description == newItem.Description);
        return descriptionEquality;
      }
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> { };
      MySqlConnection conn = DB.Connection();
      conn.Open(); // Opens a database connections
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;"; // Constructs an SQL query
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader; // Returns the query results from the database
      while (rdr.Read()) // READ() method reads results from the database one at a time and then advances to the next record. This method returns a BOOLEAN! When the method reaches the end of the records that our query has returned, it returns false and our while loop ends.
      {
        // database read results are returned in the form of indexed array!
        int itemId = rdr.GetInt32(0); // 0 = row index 0 from the database
        string itemDescription = rdr.GetString(1); // 1 = row index 1 from the database
        Item newItem = new Item(itemDescription, itemId);
        allItems.Add(newItem);
      }
      conn.Close(); // Closes the connection
      if (conn != null)
      {
        conn.Dispose(); // Best practice to close our database connection when we're done as this allows the database to reallocate resources to respond to requests from other users
      }
      return allItems;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Item Find(int searchId)
    {
      // Temporarily returning placeholder item to get beyond compiler errors until we refactor to work with database.
      Item placeholderItem = new Item("placeholder item");
      return placeholderItem;
    }

  }
}