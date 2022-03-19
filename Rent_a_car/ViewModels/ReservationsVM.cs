using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Rent_a_car.Entities;

namespace Rent_a_car.ViewModels
{
    public class ReservationsVM
    {
        [DisplayName("Car:")]
        [Required(ErrorMessage = "*This field is required!")]
        public int CarId { get; set; }
        [DisplayName("Date of reservaion:")]
        [Required(ErrorMessage = "*This field is required!")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/mm/yyyy}")]
        public DateTime DateOfReservaion { get; set; }
        [DisplayName("End of reservation:")]
        [Required(ErrorMessage = "*This field is required!")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/mm/yyyy}")]
        public DateTime EndDate { get; set; }

        public Reservations GetReservation() => new Reservations()
        {
            CarId = this.CarId,
            DateOfReservation = this.DateOfReservaion,
            EndDate = this.EndDate
        };
    }
}
