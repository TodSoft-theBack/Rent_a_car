using Rent_a_car.Entities;
using Rent_a_car.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.ValidationAttributes
{
    public class CarsYearValidationAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int Year = (int)value;
            if (Year > DateTime.Today.Year)
                return new ValidationResult("Year cannot be in the future!");
            return ValidationResult.Success;
        }
    }
}
