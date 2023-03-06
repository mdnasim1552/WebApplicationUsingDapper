namespace WebApplicationUsingDapper.Models.Account
{
    public class User
    {
        public string? userId { get; set; }
        public string fName { get; set; }
        public string? lName { get; set; }
        public string phoneNo { get; set; }
        public string emailNo { get; set; }
        public string? userCity { get; set; }
        public string? userImg { get; set; }
        public string? userCV { get; set; }
        public string password { get; set; }
        public DateTime? dob { get; set; }
        public string gender { get; set; }

    }
}
