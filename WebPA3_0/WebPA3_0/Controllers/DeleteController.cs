using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using WebPA;
using WebPA3_0.Models;
using WebPA3_0.Services;


namespace WebPA3_0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeleteController : Controller
    {
        private readonly ILogger<HomeController> _logger; 
        string postgresUsername = Environment.GetEnvironmentVariable("postgresUsername");
        string postgresPassword = Environment.GetEnvironmentVariable("postgresPassword");
        InMemoryItemService itemService = new InMemoryItemService();
        public DeleteController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Delete()
        {
            return View("Delete",itemService.RefreshItems());
        }

        [HttpPost]
        [Route("DeleteJS")]
        public void DeleteJS([FromForm] int id)
        {
            Console.WriteLine("ez a backend id: " + id);
            var host = "localhost";
            var db = "WebPA";
            var port = "54321";
            var connString = $"Host={host};Port ={ port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            using (var cmd = new NpgsqlCommand($"delete from items where item_id = {id} ", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        [HttpGet]
        [Route("GetItems")]
        public List<Items> GetItems()
        {
            return itemService.RefreshItems();
        }
    }
}
