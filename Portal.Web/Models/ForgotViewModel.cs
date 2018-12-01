using System.ComponentModel.DataAnnotations;

namespace Portal.Web.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}