using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Rent_a_car.Entities
{
    public partial class Location
    {
        public Location()
        {
            ReservationsDropOffLocation = new HashSet<Reservations>();
            ReservationsPickUpLocation = new HashSet<Reservations>();
        }

        public int Id { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Reservations> ReservationsDropOffLocation { get; set; }
        public virtual ICollection<Reservations> ReservationsPickUpLocation { get; set; }
    }
}
