using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebPA3_0.Models;
using WebPA3_0;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using WebPA3_0.Services;
using Npgsql;
using WebPA.Models;
using WebPA;

namespace WebPA3_0.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        public  InMemoryInventoryService inventoryService = new InMemoryInventoryService();
        string postgresUsername = Environment.GetEnvironmentVariable("postgresUsername");
        string postgresPassword = Environment.GetEnvironmentVariable("postgresPassword");


        public AccountController(ILogger<AccountController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginAsync([FromForm] string email, [FromForm] string password)
        {
            _userService.UpdateUsers();
            User user = _userService.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("Login","Account");
            }

            
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, email) };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties{};
            await HttpContext.SignInAsync(

                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            
            return RedirectToAction("Index", "Profile");
        }
        //public async Task<ActionResult> RegistrateinAsync([FromForm] string email, [FromForm] string password)
        //{
        //    User user = _userService.Register(email, password);
        //    if (user == null)
        //    {
        //        return RedirectToAction("Register", "Account");
        //    }


        //    var claims = new Claim[]
        //    {
        //        new Claim("email",email),
        //        new Claim("password", password)
        //    };
        //    //var claims = new List<Claim> { new Claim(ClaimTypes.Email, email) };

        //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var authProperties = new AuthenticationProperties { };
        //    await HttpContext.SignInAsync(

        //        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        //    return RedirectToAction("Index", "Profile");
        //}
        
        [HttpPost]
        public IActionResult SetUser(string email, string password)
        {

            if (email == null || password == null)
            {
                return Redirect("/Home/Login");
            }
            var host = "localhost";
            var db = "WebPA";
            var port = "54321"; 
            var connString = $"Host={host};Port ={ port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            bool _isadmin = false;
            using (var cmd = new NpgsqlCommand(
                $"insert into users(user_email, pw, isadmin) values ('{email}" +
                $"','{password}',{_isadmin})", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("Index", "Profile");


        }
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Profile");
        }

        public IActionResult Subs()
        {
            List<User> _listOfUsers = _userService.UpdateUsers();
            var email = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            int user_id = 0;
            for (int i = 0; i < _listOfUsers.Count; i++)
            {
                if (_listOfUsers[i].Email == email)
                {
                    user_id = _listOfUsers[i].Id;
                }
            }
            List<SubsInventory> subs = new List<SubsInventory>();
            subs = inventoryService.RefreshInventory();
            List<Items> subbedItems = new List<Items>();
            subbedItems= LookingForThisItem(subs, user_id);
            return View("Subs",subbedItems);
        }
        public List<Items> LookingForThisItem(List<SubsInventory> subs, int user_id)
        {
            List<Items> returnList = new List<Items>();
            InMemoryItemService itemService = new InMemoryItemService();
            List<Items> AllItems = itemService.RefreshItems();

            for (int i = 0; i < subs.Count; i++)
            {
                if (subs[i]._user_id == user_id)
                {
                    for (int j = 0; j < AllItems.Count; j++)
                    {
                        if (subs[i]._item_id==AllItems[j].item_id)
                        {
                            returnList.Add(AllItems[j]);
                        }
                    }
                }
            }
            return returnList;
        }

        [HttpPost]
        public IActionResult AddSub([FromForm] int item_id)
        {
            if (item_id==0)
            {
                return RedirectToAction("Items", "Shop");
            }

            List<User> _listOfUsers = _userService.UpdateUsers();
            var email = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            int user_id = 0;
            for (int i = 0; i < _listOfUsers.Count; i++)
            {
                if (_listOfUsers[i].Email==email)
                {
                    user_id = _listOfUsers[i].Id;
                }
            }
            List<SubsInventory> subs = new List<SubsInventory>();
            subs = inventoryService.RefreshInventory();
            for (int i = 0; i < subs.Count; i++)
            {
                if (subs[i]._user_id == user_id)
                {
                    if (subs[i]._item_id==item_id)
                    {
                        return RedirectToAction("Items", "Shop");
                    }
                }
            }




            var host = "localhost";
            var db = "WebPA";
            var port = "54321"; 
            var connString = $"Host={host};Port ={ port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            conn.Open();
            using (var cmd = new NpgsqlCommand($"INSERT INTO inventory(user_id, item_id) VALUES ({user_id}, {item_id})", conn))
            {
                cmd.ExecuteReader();
                conn.Close();
            }
            return RedirectToAction("Items", "Shop");
        }
    }
}
