using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _201400L_FinalProj.models
{
	public class ApplicationUser:  IdentityUser
	{
		[Required]
		[DataType(DataType.Text)]
		public string Full_Name { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Credit_Card_No { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Gender { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Delivery_Address { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.ImageUrl)]
		public string Photo { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string AboutMe { get; set; }
	}
}
