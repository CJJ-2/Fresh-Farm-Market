using _201400L_FinalProj.models;
using authentication.models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _201400L_FinalProj.Pages
{
    public class ConfirmedEmailModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private readonly AuthDbContext _context;
        public ConfirmedEmailModel(UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            this.userManager = userManager;
            _context = context;
        }

        [ValidateAntiForgeryToken]
        public async Task OnGetAsync(string token, string e)
        {
            try
            {
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectionProvider.CreateProtector("MySecretKey");
				var user = await userManager.FindByNameAsync(protector.Unprotect(e));
                if (user == null)
                {
                    RedirectToPage("EmailConfirmationError");
                }

           
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    user.EmailConfirmed = true;
                    _context.Update(user);
                    _context.SaveChanges();
          
                }
                else
                {
                    RedirectToPage("EmailConfirmationError");
                };
            }
            catch
            {
                 RedirectToPage("errors/ErrorGeneral");
            }
            
            
        }
    }
}
