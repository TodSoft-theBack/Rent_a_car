using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rent_a_car.Entities;
using Rent_a_car.ExtentionMethods;
using Rent_a_car.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.Controllers
{
    public class CarsController : Controller
    {
        private Rent_a_carDBContext _database;
        public CarsController(Rent_a_carDBContext database)
        {
            _database = database;
        }
        public ActionResult Index()
        {
            HttpContext.Session.SetObject("allCars", _database.Cars.ToList());
            return View();
        }
        public ActionResult AddCars()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCars(CarsVM input)
        {
            if (!this.ModelState.IsValid)
                return View(input);
            Cars car = input.GetCar();
            _database.Cars.Add(car);
            _database.SaveChanges();
            return RedirectToAction("Index","Cars");
        }
    }
}
