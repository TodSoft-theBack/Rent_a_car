using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var loggedUser = HttpContext.Session.GetObject<Users>("loggedUser");
            if (loggedUser is null)
                return RedirectToAction("LogIn", "Home");
            List<SelectListItem> pickUpLocations = new List<SelectListItem>();
            foreach (var location in _database.Location)
                pickUpLocations.Add(new SelectListItem(location.Address, location.Id.ToString()));
            List<SelectListItem> dropOffLocations = new List<SelectListItem>();
            foreach (var location in _database.Location)
                dropOffLocations.Add(new SelectListItem(location.Address, location.Id.ToString()));
            dropOffLocations.Add(new SelectListItem("<Same as pick up>", "-1", true));
            ViewBag.PickUpLocations = pickUpLocations;
            ViewBag.DropOffLocations = dropOffLocations;
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
            return RedirectToAction("Index", "Home");
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
        [HttpPost]
        public IActionResult Index(HomeQueryVM input)
        {
            var loggedUser = HttpContext.Session.GetObject<Users>("loggedUser");
            if (loggedUser is null)
                return RedirectToAction("LogIn", "Home");
            List<SelectListItem> pickUpLocations = new List<SelectListItem>();
            foreach (var location in _database.Location)
                pickUpLocations.Add(new SelectListItem(location.Address, location.Id.ToString()));
            List<SelectListItem> dropOffLocations = new List<SelectListItem>();
            foreach (var location in _database.Location)
                dropOffLocations.Add(new SelectListItem(location.Address, location.Id.ToString()));
            dropOffLocations.Add(new SelectListItem("<Same as pick up>", "-1", true));
            ViewBag.PickUpLocations = pickUpLocations;
            ViewBag.DropOffLocations = dropOffLocations;
            if (!this.ModelState.IsValid)
            {
                return View(input);
            }
            var cars = _database.Cars
                .Include(c => c.Reservations)
                .Where(c => c.Reservations.Count == 0 ||
                    c.Reservations
                    .Where(r => r.EndDate < input.PickUpDate && input.DropOffDate < r.DateOfReservation).Count() > 0).ToList();
            if (cars is null)
            {
                this.ModelState.AddModelError("stateError", "No cars avalable for this period and places!");
                return  View (input);
            }
            HttpContext.Session.SetObject<List<Cars>>("avaliableCars", cars);
            return RedirectToAction("SelectedCars", "Cars");
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
