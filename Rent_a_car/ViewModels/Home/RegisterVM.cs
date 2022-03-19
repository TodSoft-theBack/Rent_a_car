using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Rent_a_car.Entities;

namespace Rent_a_car.ViewModels
{
    public class RegisterVM
    {
        [DisplayName("Username: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string UserName { get; set; }

        [DisplayName("Password: ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Password { get; set; }

        [DisplayName("First name: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string FirstName { get; set; }

        [DisplayName("Last name: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string LastName { get; set; }

        [DisplayName("EGN: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "*EGN must only contain numbers")]
        [StringLength(10, MinimumLength = 10, ErrorMessage ="EGN must be 10 characters!")]
        public string Egn { get; set; }

        [DisplayName("Email: ")]
        [DataType(DataType.EmailAddress, ErrorMessage = "*Enter a valid email")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Email { get; set; }

        [DisplayName("Phone number: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        [StringLength(13, MinimumLength = 9, ErrorMessage ="Phone number must be 9-13 characters!")]
        public string Phone { get; set; }

        public Users GetUser() => new Users() {
            UserName = this.UserName,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Egn = this.Egn,
            Password = this.Password,
            Phone = this.Phone,
            Email = this.Email
        };
    }
}
