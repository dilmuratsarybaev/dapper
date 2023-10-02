using Dapper;
using Microsoft.Data.Sqlite;


namespace YourApp.Data
{
    public class ItemRepository

    {

        private readonly SqliteConnection _dbConnection;


        public ItemRepository (SqliteConnection dbConnection)
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
    }
}
