using System;
using System.ComponentModel;

using Rent_a_car.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rent_a_car.ViewModels
{
    public class HomeQueryVM
    {
        [DisplayName("Pick up date: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public DateTime PickUpDate { get; set; }
        [DisplayName("Pick up station: ")]
        public int PickUpStation { get; set; }
        [DisplayName("Drop off date: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public DateTime DropOffDate { get; set; }
        [DisplayName("Drop off station: ")]
        public int DropOffStation { get; set; }
    }
}
