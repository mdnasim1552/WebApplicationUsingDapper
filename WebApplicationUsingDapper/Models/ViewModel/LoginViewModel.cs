using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApplicationUsingDapper.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Enter Password")]
        public string password { get; set; }

    }
}
