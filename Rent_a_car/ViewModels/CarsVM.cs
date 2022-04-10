using Microsoft.AspNetCore.Http;
using Rent_a_car.Entities;
using Rent_a_car.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.ViewModels
{
    public class CarsVM
    {
        public static CarsVM GetCarsVM(Cars car) => new CarsVM()
        {
            Model = car.Model,
            Brand = car.Brand,
            Year = int.Parse(car.Year),
            PassengersCount = car.PassengersCount,
            Description = car.Description,
            EnginePower = car.EnginePower,
            EngineType = car.EngineType,
            GearBox = car.GearBox,
            CarType = car.CarType,
            PricePerDay = car.PricePerDay
        };
        public int Id { get; set; }
        [DisplayName("Model: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Model { get; set; }

        [DisplayName("Brand: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Brand { get; set; }

        [DisplayName("Year: ")]
        [Range(1950, 2050, ErrorMessage = "*Enter a valid year.")]
        [Required(ErrorMessage = "*This field is Required!")]
        [CarsYearValidation()]
        public int Year { get; set; }

        [DisplayName("No of passengers: ")]
        [Range(1, 8, ErrorMessage = "*Enter a valid passengers count.")]
        [Required(ErrorMessage = "*This field is Required!")]
        public int PassengersCount { get; set; }

        [DisplayName("Description: ")]
        [StringLength(255)]
        public string Description { get; set; }

        [DisplayName("Price per day: ")]
        [Range(0, double.MaxValue, ErrorMessage = "*Enter a valid price.")]
        [Required(ErrorMessage = "*This field is Required!")]
        [DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }
        [DisplayName("Gear box: ")]
        public int GearBox { get; set; }
        [DisplayName("Engine type: ")]
        public int EngineType { get; set; }
        [DisplayName("Engine power: ")]
        public int EnginePower { get; set; }
        [DisplayName("Vehicle type: ")]
        public int CarType { get; set; }

        [DisplayName("Picture: ")]
        [DataType( DataType.ImageUrl, ErrorMessage ="You must select an image file!")]
        public IFormFile Picture { get; set; }

        public Cars GetCar() => new Cars()
        {
            Model = this.Model,
            Brand = this.Brand,
            Year = this.Year.ToString(),
            PassengersCount = this.PassengersCount,
            Description = this.Description,
            EnginePower = this.EnginePower,
            EngineType = this.EngineType,
            GearBox = this.GearBox,
            CarType = this.CarType,
            PricePerDay = this.PricePerDay
        };
        public Cars GetCar(Cars car)
        {
            car.Model = this.Model;
            car.Brand = this.Brand;
            car.Year = this.Year.ToString();
            car.PassengersCount = this.PassengersCount;
            car.Description = this.Description;
            car.EnginePower = this.EnginePower;
            car.EngineType = this.EngineType;
            car.GearBox = this.GearBox;
            car.CarType = this.CarType;
            car.PricePerDay = this.PricePerDay;
            return car;
        }
    }
}
