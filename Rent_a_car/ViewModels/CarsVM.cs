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
        [Range(1950, 2022, ErrorMessage = "*Enter a valid date.")]
        [Required(ErrorMessage = "*This field is Required!")]
        [CarsYearValidation()]
        public int Year { get; set; }

        [DisplayName("No of passengers: ")]
        [Range(1, 8, ErrorMessage = "*Enter a valid passengers count.")]
        [Required(ErrorMessage = "*This field is Required!")]
        public int PassengersCount { get; set; }

        [DisplayName("Description: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Description { get; set; }

        [DisplayName("Price per day: ")]
        [Range(50, 1000, ErrorMessage = "*Enter a valid price.")]
        [Required(ErrorMessage = "*This field is Required!")]
        public decimal PricePerDay { get; set; }

        public Cars GetCar() => new Cars()
        {
            Model = this.Model,
            Brand = this.Brand,
            Year = this.Year,
            PassengersCount = this.PassengersCount,
            Description = this.Description,
            PricePerDay = this.PricePerDay
        };
    }
}
