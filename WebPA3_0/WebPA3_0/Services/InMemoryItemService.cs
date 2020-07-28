using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPA;

namespace WebPA3_0.Services
{
    public class InMemoryItemService
    {
        string postgresUsername = Environment.GetEnvironmentVariable("postgresUsername");
        string postgresPassword = Environment.GetEnvironmentVariable("postgresPassword");

        public InMemoryItemService()
        {
        }
        public List<Items> RefreshItems()
        {
            List<Items> _items = new List<Items>();
            var host = "localhost";
            var db = "WebPA";
            var port = "54321";
            var connString = $"Host={host};Port ={port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            using (var cmd = new NpgsqlCommand("select * from items", conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var item_id = int.Parse(reader["item_id"].ToString());
                    var item_name =reader["item_name"].ToString();
                    var item_price =int.Parse(reader["item_price"].ToString());
                    var sale_percent =int.Parse(reader["sale_percent"].ToString());
                    Items item = new Items(item_id, item_name, item_price, sale_percent);
                    _items.Add(item);
                }
                conn.Close();
            }
            return _items;
        }

    }
}
