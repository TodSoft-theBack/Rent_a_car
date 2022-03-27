using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ReservationsController : Controller
    {
        private Rent_a_carDBContext _database;
        List<Reservations> reservationHistory;
        public ReservationsController(Rent_a_carDBContext database)
        {
            _database = database;
        }
        //Method is async to prevent crashes if requests are being sent too fast
        #nullable enable
        public async Task<ViewResult> Index(string? Id)
        {
            //Shows all reservations by default (when id is -1), otherwise shows only reservations for the chosen car id
            int carId = Id is null ? -1 : int.Parse(Id);

            var user = HttpContext.Session.GetObject<Users>("loggedUser");
            reservationHistory = new List<Reservations>();
            //Converts the cars to dropdown list items
            IEnumerable<SelectListItem> allCars = _database.Cars.Select(car => new SelectListItem
            {
                Value = car.Id.ToString(),
                Text = car.Model
            });

            if (user.IsAdmin == 0)
                reservationHistory = _database.Reservations.Include(r => r.Car).Where(r => r.UserId == user.Id).ToList();
            else if(carId == -1)
                reservationHistory = _database.Reservations.Include(r => r.Car).ToList();
            else
                reservationHistory = _database.Reservations.Include(r => r.Car).Where(r => r.CarId == carId).ToList();

            ViewData["reservationHistory"] = reservationHistory;
            ViewData["allCars"] = allCars;

            //Adjusts the reservations' statuses
            await _database.Reservations.Where(r => r.Status == (int)Statuses.Approved && r.DateOfReservation > DateTime.Now).ForEachAsync(r => r.Status = (int)Statuses.Ongoing);
            await _database.Reservations.Where(r => r.Status == (int)Statuses.Ongoing && r.EndDate < DateTime.Now).ForEachAsync(r => r.Status = (int)Statuses.Completed);
            await _database.Reservations.Where(r => r.EndDate < DateTime.Today).ForEachAsync(r => r.Status = (int)Statuses.Missed);

            await _database.SaveChangesAsync();
            return View();
        }
        #nullable disable

        [HttpPost]
        public ActionResult ApproveReservation(int id)
        {
            var reservation = _database.Reservations.Where(r => r.Id == id).FirstOrDefault();
            reservation.Status = (int)Statuses.Approved;
            reservation.AprovedDate = DateTime.Now;
            _database.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult RejectReservation(int id)
        {
            var reservation = _database.Reservations.Where(r => r.Id == id).FirstOrDefault();
            reservation.Status = (int)Statuses.Upcoming;
            reservation.AprovedDate = null;
            _database.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult MakeReservations()
        {
            List<SelectListItem> cars = new List<SelectListItem>();
            var avalableCars = _database.Cars.Include(c => c.Reservations)
                .Where
                (c => c.Reservations
                        .Where(r => r.Status == (int)Statuses.Completed || 
                               r.Status == (int)Statuses.Missed)
                        .Count() > 0 ||
                        c.Reservations.Count() == 0
                ).ToList();
            foreach (var car in avalableCars)
            {
                cars.Add(new SelectListItem(string.Format($"{car.Brand} {car.Model} {car.Year}"), car.Id.ToString()));
            }
            ViewBag.AvalableCarsIDsList = cars;
            return View();
        }
        [HttpPost]
        public Task<ViewResult> MakeReservations(ReservationsVM input)
        {
            if (!this.ModelState.IsValid)
                return Task.FromResult(View(input));
            var user = HttpContext.Session.GetObject<Users>("loggedUser");
            var reservation = input.GetReservation();
            reservation.Status = (int)Statuses.Upcoming;
            reservation.UserId = user.Id;
            _database.Reservations.Add(reservation);
            _database.SaveChanges();
            return Index(null);
        }
    }
}
