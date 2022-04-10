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
            HttpContext.Session.SetObject("allCars", _database.Cars);
            var loggedUser = HttpContext.Session.GetObject<Users>("loggedUser");
            if (loggedUser == null)
                return RedirectToAction("LogIn", "Home");
            if (loggedUser.IsAdmin == 0)
                return RedirectToAction("Index", "Home");
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
            gearBoxes.Add(new SelectListItem("All types","-1", true));
            ViewBag.gearBoxesFilter = gearBoxes;
            ViewBag.carTypes = carTypes;
            return View();
        }
        public IActionResult AddCar()
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
        [HttpPost]
        public IActionResult FilterCars(CarFilterVM input)
        {
            HttpContext.Session.SetObject("GearBoxFilter", input);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult FilterSelectedCars(CarFilterVM input)
        {
            HttpContext.Session.SetObject("SelectedCarsFilter", input);
            return RedirectToAction("SelectedCars");
        }
        public IActionResult SelectedCars()
        {
            var input = HttpContext.Session.GetObject<HomeQueryVM>("filterData");
            var cars = _database.Cars.Include(c => c.Reservations)
                .Where(c => c.Reservations.Count == 0).ToList();
            cars.AddRange(_database.Cars.Include(c => c.Reservations)
                .Where(c => c.Reservations
                    .Where(r => (input.PickUpDate >= r.DateOfReservation && input.PickUpDate <= r.EndDate) || 
                    (input.DropOffDate >= r.DateOfReservation && input.DropOffDate <= r.EndDate)).Count() == 0 &&
                c.Reservations.OrderByDescending(r => r.DateOfReservation).FirstOrDefault().DropOffLocationId == input.PickUpStation
                ).ToList());
            if (cars is null)
            {
                this.ModelState.AddModelError("stateError", "No cars avalable for this period and places!");
                return View(input);
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
            engineTypes.Add(new SelectListItem("All types", "-1", true));
            gearBoxes.Add(new SelectListItem("All types", "-1", true));
            carTypes.Add(new SelectListItem("All types", "-1", true));
            ViewBag.engineTypes = engineTypes;
            ViewBag.gearBoxes = gearBoxes;       
            ViewBag.carTypes = carTypes;
            ViewBag.avaliableCars = cars;
            return View();
        }

        [HttpPost]
        public IActionResult AddCar(CarsVM input)
        {
            if (!this.ModelState.IsValid)
                return View(input);
            if (input.Picture is null)
            {
                this.ModelState.AddModelError("Picture", "You must choose a picture!");
                return View(input);
            }
            Cars car = input.GetCar();
            string id = Guid.NewGuid().ToString();
            string fileName = Path.Combine(_hostEnvironment.WebRootPath, "CarImages", id + Path.GetExtension(input.Picture.FileName));
            input.Picture.CopyTo(new FileStream(fileName, FileMode.Create));
            car.Picture = id + Path.GetExtension(input.Picture.FileName);
            _database.Cars.Add(car);
            _database.SaveChanges();
            return RedirectToAction("Index", "Cars");
        }
        [HttpPost]
        public IActionResult ReserveCar(int id)
        {
            var loggedUser = HttpContext.Session.GetObject<Users>("loggedUser");
            var input = HttpContext.Session.GetObject<HomeQueryVM>("filterData");
            var car = _database.Cars.Find(id);
            if (loggedUser is null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            if (car is null)
            {
                this.ModelState.AddModelError("carError", "Such car does not exist!");
                return RedirectToAction("SelectedCars");
            }
            if (input is null)
            {
                return RedirectToAction("Index", "Home");
            }
            var reservation = new Reservations()
            {
                UserId = loggedUser.Id,
                CarId = car.Id,
                DateOfReservation = input.PickUpDate,
                EndDate = input.DropOffDate,
                PickUpLocationId = input.PickUpStation
            };
            if (input.DropOffStation == -1)
                reservation.DropOffLocationId = input.PickUpStation;
            else
                reservation.DropOffLocationId = input.DropOffStation;
            _database.Reservations.Add(reservation);
            _database.SaveChanges();
            return RedirectToAction("Index", "Reservations");
        }
        public IActionResult EditCar(int? id)
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

            return View(CarsVM.GetCarsVM(car));
        }

        [HttpPost]
        public IActionResult EditCar(CarsVM input)
        {
            if (ModelState.IsValid)
            {
                Cars car = _database.Cars.Find(input.Id);
                car = input.GetCar(car);
                if (!(input.Picture  is null))
                {
                    string id = Guid.NewGuid().ToString();
                    string fileName = Path.Combine(_hostEnvironment.WebRootPath, "CarImages", id + Path.GetExtension(input.Picture.FileName));
                    input.Picture.CopyTo(new FileStream(fileName, FileMode.Create));
                    car.Picture = id + Path.GetExtension(input.Picture.FileName);
                }
                _database.SaveChanges();
                return RedirectToAction("Index", "Cars");
            }
            return EditCar(input.Id);
        }
        public IActionResult DeleteCar(int? id)
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
            return View(CarsVM.GetCarsVM(car));
        }
        [HttpPost]
        public IActionResult DeleteCar(int id)
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
