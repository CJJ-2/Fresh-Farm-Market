using _201400L_FinalProj.models;
using _201400L_FinalProj.ViewModels;
using authentication.models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Twilio.Jwt.AccessToken;
using static System.Net.Mime.MediaTypeNames;

namespace _201400L_FinalProj.Pages
{
    public class RegisterModel : PageModel
    {
		private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }

		private readonly RoleManager<IdentityRole> roleManager;

		private IWebHostEnvironment _environment;

        private readonly AuthDbContext _context;

        [BindProperty]
		public Register RModel { get; set; }

		//[BindProperty]
		//public IFormFile? Upload { get; set; }
		public RegisterModel(UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment environment, AuthDbContext context)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.roleManager = roleManager;
			_environment = environment;
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				string key = "qwertyuiopqwertyuiopqwertyuiopas";
				string IV = "qwertyuiopasdfgh";
				if (ModelState.IsValid)
				{

					var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
					var protector = dataProtectionProvider.CreateProtector("MySecretKey");
					//RijndaelManaged cipher = new RijndaelManaged();
					//cipher.KeySize = 256;
					//cipher.BlockSize = 128;
					//cipher.Key = Encoding.UTF8.GetBytes(key);
					//            cipher.IV = Encoding.UTF8.GetBytes(IV);
					//cipher.Padding = PaddingMode.PKCS7;
					//cipher.Mode = CipherMode.CBC;
					//            ICryptoTransform encryptTransform = cipher.CreateEncryptor(cipher.Key, cipher.IV);

					if (RModel.Photo != null)
					{
						var imageFile = Guid.NewGuid() + Path.GetExtension(RModel.Photo.FileName);
						var file = Path.Combine(_environment.ContentRootPath, "wwwroot\\upload", imageFile);
						using var fileStream = new FileStream(file, FileMode.Create);
						await RModel.Photo.CopyToAsync(fileStream);

						//var user = new ApplicationUser()
						//{
						//	UserName = RModel.Email,
						//	Full_Name = Convert.ToBase64String(encryptTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(HttpUtility.HtmlEncode(RModel.Full_Name)), 0, Encoding.UTF8.GetBytes(RModel.Full_Name).Length)),
						//	Credit_Card_No = protector.Protect(RModel.Credit_Card_No),
						//	Gender = Convert.ToBase64String(encryptTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(RModel.Gender), 0, Encoding.UTF8.GetBytes(RModel.Gender).Length)),
						//	PhoneNumber = Convert.ToBase64String(encryptTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(HttpUtility.HtmlEncode(RModel.Mobile_No)), 0, Encoding.UTF8.GetBytes(RModel.Mobile_No).Length)),

						//	Delivery_Address = Convert.ToBase64String(encryptTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(HttpUtility.HtmlEncode(RModel.Delivery_Address)), 0, Encoding.UTF8.GetBytes(RModel.Delivery_Address).Length)),
						//	Email = Convert.ToBase64String(encryptTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(RModel.Email), 0, Encoding.UTF8.GetBytes(RModel.Email).Length)),

						//                   Photo = Convert.ToBase64String(encryptTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(imageFile), 0, Encoding.UTF8.GetBytes(imageFile).Length)),
						//	AboutMe = Convert.ToBase64String(encryptTransform.TransformFinalBlock(Encoding.UTF8.GetBytes(HttpUtility.HtmlEncode(RModel.AboutMe)), 0, Encoding.UTF8.GetBytes(RModel.AboutMe).Length)),

						//	TwoFactorEnabled = true,
						//	PhoneNumberConfirmed = true

						//};

						var user = new ApplicationUser()
						{
							UserName = HttpUtility.HtmlEncode(RModel.Email),
							Full_Name = protector.Protect(HttpUtility.HtmlEncode(RModel.Full_Name)),
							Credit_Card_No = protector.Protect(HttpUtility.HtmlEncode(RModel.Credit_Card_No)),
							Gender = protector.Protect(HttpUtility.HtmlEncode(RModel.Gender)),
							PhoneNumber = protector.Protect(HttpUtility.HtmlEncode(RModel.Mobile_No)),
							Delivery_Address = protector.Protect(HttpUtility.HtmlEncode(RModel.Delivery_Address)),
							Email = protector.Protect(HttpUtility.HtmlEncode(RModel.Email)),
							Photo = protector.Protect(HttpUtility.HtmlEncode(imageFile)),
							AboutMe = protector.Protect(HttpUtility.HtmlEncode(RModel.AboutMe)),
							TwoFactorEnabled = true,
							PhoneNumberConfirmed = true,

						};

						//Create the User role if NOT exist
						IdentityRole role = await roleManager.FindByIdAsync("User");
						if (role == null)
						{
							IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("User"));
							if (!result2.Succeeded)
							{
								ModelState.AddModelError("", "Create role user failed");
							}
						}

						var result = await userManager.CreateAsync(user, HttpUtility.HtmlEncode(RModel.Password));
						if (result.Succeeded)
						{

							var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
							var confirmationLink = Url.Page("/ConfirmedEmail", null, new { token, e = user.Email }, Request.Scheme);
							EmailHelper emailHelper = new EmailHelper();
							bool emailResponse = emailHelper.SendEmailConfirm(user.UserName, confirmationLink);


							result = await userManager.AddToRoleAsync(user, "User");

							var auditlog = new AuditLogs
							{
								UserEmail = RModel.Email,
								Action = "User registered an account",
								TimeStamp = DateTime.Now
							};
							_context.AuditLogs.Add(auditlog);
							_context.SaveChanges();
							return RedirectToPage("Login");


						}
						else
						{
							foreach (var error in result.Errors)
							{
								ModelState.AddModelError("", error.Description);
							}
						}

					}



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
