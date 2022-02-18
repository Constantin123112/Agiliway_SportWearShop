using Microsoft.AspNetCore.Http;

namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class RegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Patronymic { get; set; }
        public string Birth { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageSrc { get; set; }

    }
}