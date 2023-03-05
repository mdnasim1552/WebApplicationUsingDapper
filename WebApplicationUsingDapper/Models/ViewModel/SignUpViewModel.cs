using System.ComponentModel.DataAnnotations;

namespace WebApplicationUsingDapper.Models.ViewModel
{
    public class SignUpViewModel
    {
        public int userId { get; set; }

        [Required]
        [Display(Name="First Name")]
        public string fName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid phone number")]
        public string phoneNo { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string emailNo { get; set; }

        [Display(Name = "City")]
        public string userCity { get; set; }

        //[Required]
        [Display(Name = "Upload Image")]
        public IFormFile? userImg { get; set; }

        //[Required]
        [Display(Name = "Upload CV")]
        public IFormFile? userCV { get; set; }

        [Required]
        [Display(Name = "Enter Password")]
        public string password { get; set; }

        [Required]
        [Display(Name = "Re-Enter Password")]
        [Compare("password", ErrorMessage = "Passwords do not match")]
        public string confirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter your birthdate")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        
        public DateTime? dob { get; set; }

        
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public List<Country> Countries { get; set; }= new List<Country>();
        public List<City> Cities { get; set; }=new List<City>();
    }
}
