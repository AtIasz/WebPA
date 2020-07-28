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
using WebPA3_0.Services;

namespace WebPA3_0.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IUserService _userService;

        public ProfileController(ILogger<ProfileController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var email = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            User user = _userService.GetOne(email);
            return View(new User ( user.Id, user.Email, user.Password,user.IsAdmin ));
        }

    }
}
