using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public ActionResult EditCars(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cars car = _database.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCars(Cars editModel)
        {
            if (ModelState.IsValid)
            {
                Cars car = new Cars()
                {
                    Id = editModel.Id,
                    Model = editModel.Model,
                    Brand = editModel.Brand,
                    Year = editModel.Year,
                    PassengersCount = editModel.PassengersCount,
                    Description = editModel.Description,
                    PricePerDay = editModel.PricePerDay
                };
                try
                {
                    _database.Update(car);
                    _database.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Cars");
            }
            return RedirectToAction("Index", "Cars");
        }
        public ActionResult DeleteCars(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCars(int id)
        {
            Cars car = _database.Cars.Find(id);
            _database.Cars.Remove(car);
            _database.SaveChanges();

            return RedirectToAction("Index", "Cars");
        }
        private bool CarExists(int id)
        {
            return _database.Cars.Any(e => e.Id == id);
        }
    }
}
