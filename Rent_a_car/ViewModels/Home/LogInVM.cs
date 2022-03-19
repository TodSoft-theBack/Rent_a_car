using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.ViewModels
{
    public class LogInVM
    {
        [DisplayName("Username: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Username { get; set; }

        [DisplayName("Password: ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Password { get; set; }
    }
}
