using System;
using System.Collections.Generic;
using System.IO;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Rent_a_car.Entities
{
    public partial class Cars
    {
        public Cars()
        {
            Reservations = new HashSet<Reservations>();
        }

        public int Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Year { get; set; }
        public int PassengersCount { get; set; }
        public string Description { get; set; }
        public int GearBox { get; set; }
        public int EngineType { get; set; }
        public int EnginePower { get; set; }
        public int CarType { get; set; }
        public string Picture { get; set; }
        public decimal PricePerDay { get; set; }

        public virtual ICollection<Reservations> Reservations { get; set; }
    }
}
