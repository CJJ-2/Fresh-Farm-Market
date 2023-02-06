using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _201400L_FinalProj.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        
        public void OnGet()
        {
        }
    }
}
