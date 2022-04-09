using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Rent_a_car.Entities;
using Rent_a_car.ExtentionMethods;
using Rent_a_car.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Rent_a_car.Controllers
{
    public class CarsController : Controller
    {
        private Rent_a_carDBContext _database;
        private IWebHostEnvironment _hostEnvironment;
        public CarsController(Rent_a_carDBContext database, IWebHostEnvironment environment)
        {
            _database = database;
            _hostEnvironment = environment;
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetObject("allCars", _database.Cars.ToList());
            var loggedUser = HttpContext.Session.GetObject<Users>("loggedUser");
            if (loggedUser == null)
                return RedirectToAction("LogIn", "Home");
            List<SelectListItem> engineTypes = new List<SelectListItem>();
            List<SelectListItem> gearBoxes = new List<SelectListItem>();
            List<SelectListItem> carTypes = new List<SelectListItem>();
            for (int i = 0; i < 4; i++)
                engineTypes.Add(new SelectListItem(((EngineTypes)i).ToString(), i.ToString()));
            for (int i = 0; i < 2; i++)
                gearBoxes.Add(new SelectListItem(((GearBoxes)i).ToString(), i.ToString()));
            for (int i = 0; i < 5; i++)
                carTypes.Add(new SelectListItem(((CarTypes)i).ToString(), i.ToString()));
            ViewBag.engineTypes = engineTypes;
            ViewBag.gearBoxes = gearBoxes;
            ViewBag.carTypes = carTypes;
            return View();
        }
        public IActionResult AddCars()
        {
            List<SelectListItem> engineTypes = new List<SelectListItem>();
            List<SelectListItem> gearBoxes = new List<SelectListItem>();
            List<SelectListItem> carTypes = new List<SelectListItem>();
            for (int i = 0; i < 4; i++)
                engineTypes.Add(new SelectListItem(((EngineTypes)i).ToString(), i.ToString()));
            for (int i = 0; i < 2; i++)
                gearBoxes.Add(new SelectListItem(((GearBoxes)i).ToString(), i.ToString()));
            for (int i = 0; i < 5; i++)
                carTypes.Add(new SelectListItem(((CarTypes)i).ToString(), i.ToString()));
            ViewBag.engineTypes = engineTypes;
            ViewBag.gearBoxes = gearBoxes;
            ViewBag.carTypes = carTypes;
            return View();
        }
        public IActionResult SelectedCars()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddCars(CarsVM input)
        {
            if (!this.ModelState.IsValid)
                return View(input);
            if (input.Picture is null)
            {
                this.ModelState.AddModelError("Picture","You must choose a picture!");
                return View(input);
            }
            Cars car = input.GetCar();
            string id = Guid.NewGuid().ToString();
            string fileName = Path.Combine(_hostEnvironment.WebRootPath,"CarImages", id  + Path.GetExtension(input.Picture.FileName)) ;
            input.Picture.CopyTo(new FileStream(fileName, FileMode.Create));
            car.Picture = id + Path.GetExtension(input.Picture.FileName);
            _database.Cars.Add(car);
            _database.SaveChanges();
            return RedirectToAction("Index","Cars");
        }
        public IActionResult EditCars(int? id)
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
            List<SelectListItem> engineTypes = new List<SelectListItem>();
            List<SelectListItem> gearBoxes = new List<SelectListItem>();
            List<SelectListItem> carTypes = new List<SelectListItem>();
            for (int i = 0; i < 4; i++)
                engineTypes.Add(new SelectListItem(((EngineTypes)i).ToString(), i.ToString()));
            for (int i = 0; i < 2; i++)
                gearBoxes.Add(new SelectListItem(((GearBoxes)i).ToString(), i.ToString()));
            for (int i = 0; i < 5; i++)
                carTypes.Add(new SelectListItem(((CarTypes)i).ToString(), i.ToString()));
            ViewBag.engineTypes = engineTypes;
            ViewBag.gearBoxes = gearBoxes;
            ViewBag.carTypes = carTypes;
            return View(new CarsVM(car));
        }

        [HttpPost]
        public IActionResult EditCars(Cars editModel)
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
        public IActionResult DeleteCars(int? id)
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
            return View(new CarsVM(car));
        }
        [HttpPost]
        public IActionResult DeleteCars(int id)
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
