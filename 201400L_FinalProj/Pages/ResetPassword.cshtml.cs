using _201400L_FinalProj.models;
using _201400L_FinalProj.ViewModels;
using authentication.models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace _201400L_FinalProj.Pages
{
    public class ResetPasswordModel : PageModel
    {
		private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }

		private readonly AuthDbContext _context;
		public ResetPasswordModel(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, AuthDbContext context)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			_context = context;
		}

		[BindProperty]
		public ResetPassword RPM { get; set; }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectionProvider.CreateProtector("MySecretKey");
				if (!ModelState.IsValid)
				{
					return Page();
				}

				var user = await userManager.FindByNameAsync(protector.Unprotect(RPM.Email_to_reset));
				if (user == null)
				{
					return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
				}

				var changePasswordResult = await userManager.ResetPasswordAsync(user, RPM.Token, HttpUtility.HtmlEncode(RPM.PasswordReset));
				if (!changePasswordResult.Succeeded)
				{
					foreach (var error in changePasswordResult.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
					return Page();
				}
				else
				{
					var auditlog = new AuditLogs
					{
						UserEmail = RPM.Email_to_reset,
						Action = "User resetted password",
						TimeStamp = DateTime.Now
					};
					_context.AuditLogs.Add(auditlog);
					_context.SaveChanges();
				

					await userManager.SetLockoutEnabledAsync(user, false);

					await userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(10));
					

					return RedirectToPage("Login");
				}
			}
			catch
			{
				return Redirect("errors/ErrorGeneral");
			}


		}
		public void OnGet()
        {
        }
    }
}
