using Rent_a_car.Entities;
using Rent_a_car.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.ViewModels
{
    public class CarsVM
    {
        [DisplayName("Model: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Model { get; set; }

        [DisplayName("Brand: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Brand { get; set; }

        [DisplayName("Year: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        [CarsYearValidation()]
        public int Year { get; set; }

        [DisplayName("No of passengers: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public int PassengersCount { get; set; }

        [DisplayName("Describtion: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Describtion { get; set; }

        [DisplayName("Price per day: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public decimal PricePerDay { get; set; }

        public Cars GetCar() => new Cars()
        {
            Model = this.Model,
            Brand = this.Brand,
            Year = this.Year,
            PassengersCount = this.PassengersCount,
            Describtion = this.Describtion,
            PricePerDay = this.PricePerDay
        };
    }
}
