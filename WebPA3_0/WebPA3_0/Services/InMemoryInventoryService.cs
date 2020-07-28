using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPA;
using WebPA.Models;

namespace WebPA3_0.Services
{
    public class InMemoryInventoryService
    {
        List<SubsInventory> _ListOfSubs = new List<SubsInventory>();
        string postgresUsername = Environment.GetEnvironmentVariable("postgresUsername");
        string postgresPassword = Environment.GetEnvironmentVariable("postgresPassword");

        public InMemoryInventoryService()
        {
            _ListOfSubs = RefreshInventory();
        }
        public List<SubsInventory> RefreshInventory()
        {
            List<SubsInventory> empty = new List<SubsInventory>();
            _ListOfSubs = empty;
            var host = "localhost";
            var db = "WebPA";
            var port = "54321";
            var connString = $"Host={host};Port={port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            using (var cmd = new NpgsqlCommand("select * from inventory", conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var user_id = (int)reader["user_id"];
                    var item_id = (int)reader["item_id"];
                    SubsInventory inventory = new SubsInventory(item_id, user_id);
                    _ListOfSubs.Add(inventory);
                }
                conn.Close();
            }
            return _ListOfSubs;
        }

    }
}
