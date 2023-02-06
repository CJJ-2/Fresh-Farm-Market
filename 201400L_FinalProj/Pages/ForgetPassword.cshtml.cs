using _201400L_FinalProj.models;
using _201400L_FinalProj.ViewModels;
using authentication.models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Twilio.TwiML.Messaging;

namespace _201400L_FinalProj.Pages
{
    public class ForgetPasswordModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        public ForgetPasswordModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [BindProperty]
        public string ForgotPw_Email { get; set; }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await userManager.FindByNameAsync(HttpUtility.HtmlEncode(ForgotPw_Email));
                    Console.WriteLine(user + "hello");
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var callback = Url.Page("/resetpassword", null, new { token, e = user.Email }, Request.Scheme);
                    EmailHelper emailhelper = new EmailHelper();
                    bool emailresponse = emailhelper.SendEmailForgetPassword(ForgotPw_Email, callback);

                    return Redirect("Index");
                }

                return Page();
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
