using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Npgsql;
using WebPA;
using WebPA3_0.Models;
using WebPA3_0.Services;
using System.Web;

namespace WebPA3_0.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string postgresUsername = Environment.GetEnvironmentVariable("postgresUsername");
        string postgresPassword = Environment.GetEnvironmentVariable("postgresPassword");
        InMemoryItemService itemService = new InMemoryItemService();
        List<Items> items = new List<Items>();
        public ShopController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Items()
        {
            return View("Items", itemService.RefreshItems());
        }
        public IActionResult MenuAdmin()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult DeletDis([FromForm] int item_id)
        {
            var host = "localhost";
            var db = "WebPA";
            var port = "54321"; 
            var connString = $"Host={host};Port ={ port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            using (var cmd = new NpgsqlCommand($"delete from items where item_id = {item_id} ", conn))
            {
                
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            items = itemService.RefreshItems();
            return View("Delete", items);
        } 
        [HttpPost]
        public IActionResult AddOne(string item_name, string price)
        {
            if (item_name == null || price == null)
            {
                return Redirect("/Shop/Add");
            }
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item_name==item_name)
                {
                    //popup: egyezik a név, változtassa meg. ok-ra kattintva átirányítja az addone-ra.
                    return View("Items", items);

                }
            }
            var host = "localhost";
            var db = "WebPA";
            var port = "54321";
            var connString = $"Host={host};Port ={ port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            int sale = 0;
            using (var cmd = new NpgsqlCommand(
                $"insert into items(item_name,item_price,sale_percent) values ('{item_name}',{Convert.ToInt32(price)},{sale})", conn))
            {

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            items = itemService.RefreshItems();
            return View("List",items);
        }
        [HttpPost]
        public IActionResult Purchase(int item_id)
        {
            Items it = new Items();
            items = itemService.RefreshItems();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item_id==item_id)
                {
                    Items tmpIt = new Items(items[i].item_id, items[i].item_name, items[i].item_price, items[i].sale_percent);
                    it = tmpIt;
                }
            }
            return View("Purchase",it);
        }
        public IActionResult AddAnotherOne(string item_name, string price)
        {
            if (item_name == null || price == null)
            {
                return Redirect("/Home/Login");
            }
            var host = "localhost";
            var db = "WebPA";
            var port = "54321";
            var connString = $"Host={host}; Port ={ port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            int sale = 0;
            using (var cmd = new NpgsqlCommand(
                $"insert into items(item_name,item_price,sale_percent) values ('{item_name}',{Convert.ToInt32(price)},{sale})", conn))
            {

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            items = itemService.RefreshItems();
            return RedirectToAction("Add", "Shop");

        }
        public IActionResult UpdateView()
        {
            items = itemService.RefreshItems();
            return View("Update", items);
        }
        public IActionResult Update(string item_name, int price, int sale)
        {
            if (item_name==null)
            {
                items = itemService.RefreshItems();
                return View("Update", items);
            }

            var host = "localhost";
            var db = "WebPA";
            var port = "54321";
            var connString = $"Host={host};Port ={ port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            //int priceIn = 0;
            //int saleIn = 0;
            Items it = new Items();
            if (price==0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].item_name==item_name)
                    {
                        it.item_name = item_name;
                        it.item_price = items[i].item_price;
                        it.sale_percent = sale;

                    }
                }
            }
            

            //(item_name,item_price,sale_percent) values('{item_name}',{price},{sale} HOZZÁADNI)
            using (var cmd = new NpgsqlCommand($"update items set item_price = {price}, sale_percent={sale} where item_name ='{item_name}'", conn))
            {

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            items = itemService.RefreshItems();
            return View("Update", items);
        }
        public IActionResult List()
        {
            return View("List", itemService.RefreshItems());
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        
    }
}
