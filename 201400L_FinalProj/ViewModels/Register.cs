using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace _201400L_FinalProj.ViewModels
{
    public class Register
    {

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("^[a-zA-Z]{4,}(?: [a-zA-Z]+){0,2}$", ErrorMessage="Full Name can only have letters and numbers")]
        public string Full_Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
		[RegularExpression("^(?:-(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))|(?:0|(?:[1-9](?:\\d{0,2}(?:,\\d{3})+|\\d*))))(?:.\\d+|)$", ErrorMessage = "Credit Card Number can only have numbers")]
		public string Credit_Card_No { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$", ErrorMessage = "Mobile Number can only have numbers")]
        public string Mobile_No { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("^(\\d{1,}) [a-zA-Z0-9\\s]+(\\,)? [a-zA-Z]+ [0-9]{5,6}$", ErrorMessage = "Delivery address can only have letters and numbers")]
        public string Delivery_Address { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", ErrorMessage = "Field can only have letters and numbers and @")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).{12,}$", ErrorMessage = "Password must have: Minimum 12 chars,\r\nUse combination of lower-case, upper-case, Numbers\r\nand special characters")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
		public IFormFile? Photo { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string AboutMe { get; set; }
    }
}
