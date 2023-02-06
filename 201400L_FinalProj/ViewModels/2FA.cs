using System.ComponentModel.DataAnnotations;

namespace _201400L_FinalProj.ViewModels
{
    public class _2FA
    {
        
        [Required]
  
        public string TwoFA { get; set; }
        
        public string Email { get; set; }
    }
}
