using Microsoft.AspNetCore.Http;

namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public string Seasonal_clothes { get; set; } // { autumn || winter || spring || summer }
        public string CategoryNormalizedName { get; set; }
        public string Status { get; set; } // Active, Not_active
        public string Size { get; set; } 
        public int Count { get; set; }
        public IFormFile formFile { get; set; }
        public IFormFileCollection formFileCollection { get; set; }
    }
}