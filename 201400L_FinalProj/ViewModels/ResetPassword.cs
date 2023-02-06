using System.ComponentModel.DataAnnotations;

namespace _201400L_FinalProj.ViewModels
{
	public class ResetPassword
	{
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).{12,}$", ErrorMessage = "Password must have: Minimum 12 chars,\r\nUse combination of lower-case, upper-case, Numbers\r\nand special characters")]
		public string PasswordReset { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(PasswordReset), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPasswordReset { get; set; }

		public string Email_to_reset { get; set; }

		public string Token { get; set; }
	}
}
