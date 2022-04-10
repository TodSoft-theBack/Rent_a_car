using Rent_a_car.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.ViewModels
{
    public class CarFilterVM
    {
        public int EngineType { get; set; }
        public int CarType { get; set; }
        public int GearBox { get; set; }
    }
}
