using _201400L_FinalProj.models;
using _201400L_FinalProj.ViewModels;
using authentication.models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace _201400L_FinalProj.Pages
{
    public class LogoutModel : PageModel
    {
        [BindProperty]
        public string logout_email { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor contxt;
        private readonly AuthDbContext _context;
        public LogoutModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, AuthDbContext context)
		{
			this.signInManager = signInManager;
            contxt = httpContextAccessor;
            _context = context;
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostLogoutAsync()
		{
            try
            {
                //RijndaelManaged cipher = new RijndaelManaged();
                //cipher.KeySize = 256;
                //cipher.BlockSize = 128;
                //cipher.Key = Encoding.UTF8.GetBytes("qwertyuiopqwertyuiopqwertyuiopas");
                //cipher.IV = Encoding.UTF8.GetBytes("qwertyuiopasdfgh");
                //cipher.Padding = PaddingMode.PKCS7;
                //cipher.Mode = CipherMode.CBC;

                //ICryptoTransform decryptTransform = cipher.CreateDecryptor(cipher.Key, cipher.IV);

                //var Emaildecode = ASCIIEncoding.ASCII.GetString(decryptTransform.TransformFinalBlock(Convert.FromBase64String(logout_email), 0, Convert.FromBase64String(logout_email).Length));
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                var Emaildecode = protector.Unprotect(logout_email);
                var auditlog = new AuditLogs
                {
                    UserEmail = Emaildecode,
                    Action = "User logged out",
                    TimeStamp = DateTime.Now
                };
                _context.AuditLogs.Add(auditlog);
                _context.SaveChanges();
                await signInManager.SignOutAsync();
                contxt.HttpContext.Session.Clear();
                return RedirectToPage("Login");
            }
			catch
			{
				return Redirect("errors/ErrorGeneral");
			}
		}
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
		public void OnGet()
        {
        }
    }
}
