using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.ViewModels
{
    public class ReservationsVM
    {

        public int CarId { get; set; }
        public int UserId { get; set; }
        public DateTime DateOfReservation { get; set; }
        public DateTime AprovedDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
}
