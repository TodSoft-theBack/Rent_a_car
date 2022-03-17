using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rent_a_car.Entities;
using Rent_a_car.ExtentionMethods;
using Rent_a_car.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.Controllers
{
    public class HomeController : Controller
    {
        private Rent_a_carDBContext _database;

        //Should be an interface but the version doesn't contain one
        public HomeController(Rent_a_carDBContext database)
        {
            _database = database;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM input)
        {
            if (!this.ModelState.IsValid)
                return View(input);
            Users user = input.GetUser();
            user.IsAdmin = 0;
            _database.Users.Add(user);
            _database.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        public IActionResult LogIn()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult LogIn(LogInVM input)
        {
            if (!this.ModelState.IsValid)
                return View(input);
            var user = _database.Users.Where(u => u.UserName == input.Username || u.Email == input.Username).FirstOrDefault();
            if (user == null)
            {
                this.ModelState.AddModelError("authError", "Such user doesn\'t exist!");
                return View(input);
            }
            if (user.Password != input.Password)
            {
                this.ModelState.AddModelError("authError", "Wrong password try again!");
                return View(input);
            }
            HttpContext.Session.SetObject("loggedUser", user);
            return RedirectToAction("Index","Home");
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("loggedUser");
            return RedirectToAction("Index", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
