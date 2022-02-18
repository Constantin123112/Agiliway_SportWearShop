namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class LoginDto 
    {
        public string Email { get; set; }
        public string Password { get; set; }
        // update pass
        public string NewPassword { get; set; }
        // check admin
        public string EmailSendPass { get; set; }
    }
}