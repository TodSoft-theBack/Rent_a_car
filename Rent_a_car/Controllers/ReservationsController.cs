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
        public ReservationsController(Rent_a_carDBContext database)
        {
            _database = database;
        }
        public ActionResult Index()
        {
            var user = HttpContext.Session.GetObject<Users>("loggedUser");
            if (user == null)
                return RedirectToAction("LogIn", "Home");
            List<Reservations> reservationHistory = new List<Reservations>();
            if (user.IsAdmin == 0)
                reservationHistory = _database.Reservations.Include(r => r.Car).Where(r => r.UserId == user.Id).ToList();
            else
                reservationHistory = _database.Reservations.Include(r => r.Car).ToList();
            ViewData["reservationHistory"] = reservationHistory;

            foreach (var reservation in _database.Reservations.Where(r => r.Status == (int)Statuses.Approved && r.DateOfReservation < DateTime.Today))
                reservation.Status = (int)Statuses.Ongoing;
            foreach (var reservation in _database.Reservations.Where(r => r.Status == (int)Statuses.Upcoming && r.DateOfReservation < DateTime.Today))
                reservation.Status = (int)Statuses.Missed;
            _database.SaveChanges();
            return View();
        }

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
        public ActionResult DisapproveReservation(int id)
        {
            var reservation = _database.Reservations.Find(id);
            reservation.Status = (int)Statuses.Upcoming;
            reservation.AprovedDate = null;
            _database.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult RejectReservation(int id)
        {
            var reservation = _database.Reservations.Find(id);
            _database.Reservations.Remove(reservation);
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
        public ActionResult MakeReservations(ReservationsVM input)
        {
            if (!this.ModelState.IsValid)
                return View(input);
            var user = HttpContext.Session.GetObject<Users>("loggedUser");
            var reservation = input.GetReservation();
            reservation.Status = (int)Statuses.Upcoming;
            reservation.UserId = user.Id;
            _database.Reservations.Add(reservation);
            _database.SaveChanges();
            return Index();
        }
    }
}
