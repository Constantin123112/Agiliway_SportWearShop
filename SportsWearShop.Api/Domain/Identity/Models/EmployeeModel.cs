using Microsoft.AspNetCore.Http;

namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class EmployeeModel
    {
        
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string Occupation { get; set; }

        public string ImageName { get; set; }

        public IFormFile ImageFile { get; set; }

        public string ImageSrc { get; set; }
    }
}
