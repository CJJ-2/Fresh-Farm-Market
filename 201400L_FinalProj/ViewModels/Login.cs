using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Twilio.TwiML.Voice;

namespace _201400L_FinalProj.ViewModels
{
    public class Login
    {
        [Required]
        [EmailAddress]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", ErrorMessage = "Field can only have letters and numbers and @")]

        public string Email { get; set; }
        [Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
        public bool RememberMe { get; set; }

      
    }
}
