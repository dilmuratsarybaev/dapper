using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace YourApp.Data
{
    public class ItemRepository

    {

        private readonly IDbConnection _dbConnection;


        public ItemRepository (IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IEnumerable<Item> GetAllItems()
        {
            _dbConnection.Open();
            var items = _dbConnection.Query<Item>("SELECT * FROM Items");
            _dbConnection.Close();

            return items;
        }

        public void DeleteItem(int itemId)
        {
            _dbConnection.Open();
            _dbConnection.Execute("DELETE FROM Items WHERE Id = @Id", new { Id = itemId });
            _dbConnection.Close();
        }

        public void InitializeDatabase ()
        {
            _dbConnection.Open ();
            _dbConnection.Execute(@"
            CREATE TABLE IF NOT EXISTS Items (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT
            )");

        }

        public void AddItem (Item item)
        {
            _dbConnection.Execute("INSERT INTO Items (Name, Description) VALUES (@Name, @Description)", item);
        }

        public void UpdateItem(Item item)
        {
            _dbConnection.Open();
            _dbConnection.Execute("UPDATE Items SET Name = @Name, Description = @Description WHERE Id = @Id", item);
            _dbConnection.Close();
        }
        public Item GetItemById(int itemId)
        {
            _dbConnection.Open();
            var item = _dbConnection.QueryFirstOrDefault<Item>("SELECT * FROM Items WHERE Id = @Id", new { Id = itemId });
            _dbConnection.Close();

            return item;
        }
    }
}
