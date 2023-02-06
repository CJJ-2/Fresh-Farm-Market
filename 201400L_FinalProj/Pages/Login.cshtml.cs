using _201400L_FinalProj.models;
using _201400L_FinalProj.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing.Printing;
using System.Net;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Web;
using System.Xml.Linq;

namespace _201400L_FinalProj.Pages
{
	public class LoginModel : PageModel
	{
		

		private readonly SignInManager<ApplicationUser> signInManager;
		private UserManager<ApplicationUser> userManager;
		private readonly IHttpContextAccessor contxt;

		public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			contxt = httpContextAccessor;

		}

		[BindProperty]
		public Login Lmodel { get; set; }

		//public string ReturnUrl { get; set; }
		//public bool ValidateCaptcha()
		//{
		//string Response = Request["g-recaptcha-response"]; //Getting Response String Append to Post Method
		//	bool Valid = false;

		//HttpWebRequest req = (HttpWebRequest)WebRequest.Create(" https://y.google.com/recaptcha/api/sitevenify?secret=6Le7ezQkAAAAAGlJ1S9BQAHzEW3HuoShtR5G0akX &response = " + Response);
		//	try
		//	{

		//		//Google recaptcha Response
		//		using (WebResponse WResponse = req.GetResponse())
		//		{
		//			using (StreamReader readStream = new StreamReader(WResponse.GetResponseStream()))
		//			{
		//				string jsonResponse = readStream.ReadToEnd();
		//				JavaScriptSerializer js = new JavaScriptSerializer();
		//				Object data = js.Deserialize <XObject›(jsonResponse); // Deserialize Json
		//				Valid = true;
		//			}
		//		}

		//		return Valid;
		//	}
		//	catch(WebException ex)
		//	{
		//		throw ex;
		//	}
		//}

		public static bool ReCaptchaPassed(string gRecaptchaResponse)
		{
			HttpClient httpClient = new HttpClient();

			var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Le7ezQkAAAAAGlJ1S9BQAHzEW3HuoShtR5G0akX &response= " + gRecaptchaResponse).Result;

			if (res.StatusCode != HttpStatusCode.OK)
			{
				return false;
			}
			string JSONres = res.Content.ReadAsStringAsync().Result;
			dynamic JSONdata = JObject.Parse(JSONres);

			if (JSONdata.success != "true" || JSONdata.score <= 0.5m)
			{
				return false;
			}

			return true;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				if (ModelState.IsValid)
				{

					ApplicationUser appUser = await userManager.FindByNameAsync(HttpUtility.HtmlEncode(Lmodel.Email));
					if (appUser != null)
					{
						await signInManager.SignOutAsync();
						Console.WriteLine(ReCaptchaPassed(Request.Form["foo"]));
						var identityResult = await signInManager.PasswordSignInAsync(appUser, Lmodel.Password, Lmodel.RememberMe, lockoutOnFailure: true);
						Console.WriteLine(identityResult.RequiresTwoFactor + "2FA");
						if (identityResult.Succeeded)
						{

							//var user = await userManager.GetUserAsync(User);
							//var user = new ApplicationUser
							//{
							//	UserName = userDetails.UserName,
							//	Full_Name = userDetails.Full_Name,
							//	Credit_Card_No = userDetails.Credit_Card_No,
							//	Gender = userDetails.Gender,
							//	Mobile_No = userDetails.Mobile_No,

							//	Delivery_Address = userDetails.Delivery_Address,
							//	Email = userDetails.Email,
							//	Password = userDetails.Password,
							//	Photo = userDetails.Photo,
							//	AboutMe = userDetails.AboutMe,
							//};

							//signInManager.SignOutAsync();


							//ReturnUrl ??= Url.Content("~/");
							//Console.WriteLine(user.UserName);


							//contxt.HttpContext.Session.SetString("Email", LModel.Email);
							//contxt.HttpContext.Session.SetString("Password", LModel.Password);
							//contxt.HttpContext.Session.SetString("token", etoken);
							return LocalRedirect("/");
						}



						if (identityResult.RequiresTwoFactor)
						{


							return RedirectToPage("TwoFactorAuthentication", new { e = appUser.Email });
						}

						if (identityResult.IsLockedOut)
						{
							ModelState.AddModelError("", "Your account is locked out. Kindly wait for 10 minutes and try again");
						}

					}

					ModelState.AddModelError("", "Username or Password incorrect or Email not confirmed");

				}
				else
				{
					string validationErrors = string.Join(",",
						ModelState.Values.Where(E => E.Errors.Count > 0)
						.SelectMany(E => E.Errors)
						.Select(E => E.ErrorMessage)
						.ToArray());
					Console.WriteLine(validationErrors);
					if (!ReCaptchaPassed(Request.Form["foo"]))
					{
						ModelState.AddModelError(string.Empty, "You failed the CAPTCHA.");
						return Page();
					}
				}
				return Page();
			}
			catch
			{
				return Redirect("errors/ErrorGeneral");
			}

		}

		public async Task OnGet()
        {

			
		}
    }
}
