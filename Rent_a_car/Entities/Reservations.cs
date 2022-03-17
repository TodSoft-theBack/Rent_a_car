using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Rent_a_car.Entities
{
    public partial class Reservations
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
        public DateTime DateOfReservation { get; set; }
        public DateTime? AprovedDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }

        public virtual Cars Car { get; set; }
        public virtual Users User { get; set; }
    }
}
