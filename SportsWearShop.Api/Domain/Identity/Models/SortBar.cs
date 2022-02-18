using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class SortBar
    {
        public string Price1 { get; set; }
        public decimal Price { get; set; }
        public static string Price1_ { get; set; } = "all";
        public string Price2 { get; set; }
        public static string Price2_ { get; set; } = "all";
        public string Size { get; set; }
        public static string Size_ { get; set; } = "all";
        public string Gender { get; set; }
        public static string Gender_ { get; set; } = "all";
        public string Categories { get; set; }
        public static string Categories_ { get; set; } = "all";
        public string Color { get; set; }
        public static string Color_ { get; set; } = "all";  
        public string KindOfSport { get; set; }
        public static string KindOfSport_ { get; set; } = "all";
        public string CategoryNormalizedName { get; set; }
        public static string CategoryNormalizedName_ { get; set; } = "all";
        public string Seasonal_clothes { get; set; }
        public static string Seasonal_clothes_ { get; set; } = "all";

        public SortBar()
        {

        }
        public SortBar(decimal price, string size, string gender, string color, string seasonal_clothes)
        {
            Price = price;
            Size = size;
            Gender = gender;
            Color = color;
            Seasonal_clothes = seasonal_clothes;
        }
        public SortBar(string price, string size, string gender, string categories, string color, string kindOfSport, string categoryNormalizedName, string seasonal_clothes)
        {
            Price1 = price;
            Size = size;
            Gender = gender;
            Categories = categories;
            Color = color;
            KindOfSport = kindOfSport;
            CategoryNormalizedName = categoryNormalizedName;
            Seasonal_clothes = seasonal_clothes;
        }        
    }
}
