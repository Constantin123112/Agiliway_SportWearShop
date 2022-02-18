using SportsWearShop.Api.Domain.Identity.Models;
using System;
using System.Collections.Generic;

namespace SportsWearShop.Api.DataAccess.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }        
        public string Color { get; set; }
        public string Gender { get; set; }
        public int Quantity { get; set; }
        public string Seasonal_clothes { get; set; }
        public string Status { get; set; } // Active, Not_active
        public string Reason { get; set; } 

        public string Size { get; set; }
        public List<PictureEntity> Photos { get; set; }

        public ProductEntity()
        {

        }

        public ProductEntity(long id, string name, string normalizedName, decimal price, string description, string Size, int quantity, string seasonal_clothes, string color, string gender, DateTime createdAt, string Status, FileDto photos)
        {
            Id = id;
            Name = name;
            NormalizedName = normalizedName;
            Price = price;
            Description = description;
            Quantity = quantity;
            Seasonal_clothes = seasonal_clothes;
            Color = color;
            Gender = gender;
            CreatedAt = createdAt;
            this.Size = Size;
            this.Status = Status;
        }
        public ProductEntity(long id, string name, string normalizedName, decimal price, string description, string Size, int quantity, string seasonal_clothes, string color, string gender, DateTime createdAt, string Status, FileDto photos, string reason)
        {
            Id = id;
            Name = name;
            NormalizedName = normalizedName;
            Price = price;
            Description = description;
            Quantity = quantity;
            Seasonal_clothes = seasonal_clothes;
            Color = color;
            Gender = gender;
            CreatedAt = createdAt;
            this.Size = Size;
            this.Status = Status;
            Reason = reason;
        }
    }

    public class ProductEntityResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Seasonal_clothes { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<FileDto> Photos { get; set; }

        public ProductEntityResult()
        {

        }

        public ProductEntityResult(int id, string name, string normalizedName, decimal price, string description, int quantity, string seasonal_clothes, DateTime createdAt, List<FileDto> photos)
        {
            Id = id;
            Name = name;
            NormalizedName = normalizedName;
            Price = price;
            Description = description;
            Quantity = quantity;
            Seasonal_clothes = seasonal_clothes;
            CreatedAt = createdAt;
            Photos = photos;
        }
    }
}