using _201400L_FinalProj.models;
using _201400L_FinalProj.ViewModels;
using authentication.models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace _201400L_FinalProj.Pages
{
    public class TwoFactorAuthenticationModel : PageModel
    {
        [BindProperty]
        public _2FA TwoFA { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor contxt;
        private readonly AuthDbContext _context;
        public TwoFactorAuthenticationModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, AuthDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
            _context = context;
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            //RijndaelManaged cipher = new RijndaelManaged();
            //cipher.KeySize = 256;
            //cipher.BlockSize = 128;
            //cipher.Key = Encoding.UTF8.GetBytes("qwertyuiopqwertyuiopqwertyuiopas");
            //cipher.IV = Encoding.UTF8.GetBytes("qwertyuiopasdfgh");
            //cipher.Padding = PaddingMode.PKCS7;
            //cipher.Mode = CipherMode.CBC;

            //ICryptoTransform decryptTransform = cipher.CreateDecryptor(cipher.Key, cipher.IV);

            //var Emaildecode = ASCIIEncoding.ASCII.GetString(decryptTransform.TransformFinalBlock(Convert.FromBase64String(TwoFA.Email), 0, Convert.FromBase64String(TwoFA.Email).Length));
            try
            {
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                var Emaildecode = protector.Unprotect(HttpUtility.HtmlEncode(TwoFA.Email));
                Console.WriteLine(Emaildecode);
                var result = await signInManager.TwoFactorSignInAsync("Email", HttpUtility.HtmlEncode(TwoFA.TwoFA), false, false);
                if (result.Succeeded)
                {

                    var username = await userManager.FindByNameAsync(Emaildecode);
                    var auditlog = new AuditLogs
                    {
                        UserEmail = username.UserName,
                        Action = "User logged in",
                        TimeStamp = DateTime.Now
                    };
                    _context.AuditLogs.Add(auditlog);
                    _context.SaveChanges();
                    contxt.HttpContext.Session.SetString("Username", username.UserName);
                    return RedirectToPage("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                    return Page();
                }
            }
            catch(Exception ex)
            {
                return RedirectToPage("errors/ErrorGeneral");
            }
        }

        [ValidateAntiForgeryToken]
        public async Task OnGetAsync(string e)
        {
            //RijndaelManaged cipher = new RijndaelManaged();
            //cipher.KeySize = 256;
            //cipher.BlockSize = 128;
            //cipher.Key = Encoding.UTF8.GetBytes("qwertyuiopqwertyuiopqwertyuiopas");
            //cipher.IV = Encoding.UTF8.GetBytes("qwertyuiopasdfgh");
            //cipher.Padding = PaddingMode.PKCS7;
            //cipher.Mode = CipherMode.CBC;

            //ICryptoTransform decryptTransform = cipher.CreateDecryptor(cipher.Key, cipher.IV);

            //var Emaildecode = ASCIIEncoding.ASCII.GetString(decryptTransform.TransformFinalBlock(Convert.FromBase64String(e), 0, Convert.FromBase64String(e).Length));
            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");
            var Emaildecode = protector.Unprotect(e);

            var user = await userManager.FindByNameAsync(Emaildecode);
            
            var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");

            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmail(user.UserName, token);
            Console.WriteLine("hello");
        }
    }
}
