using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportsWearShop.Api.DataAccess;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeService(IConfiguration configuration, ApiDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<ProductEntity> GetProductsSeasonalClothes(string seasonal_clothes)
        {
            List<ProductEntity> list = new List<ProductEntity>();

            //var products = from p in _context.CategoryProducts
            //               join c in _context.Products on p.ProductId equals c.Id
            //               join v in _context.Pictures on с.PictureId equals v.Id

            var products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           //join v in _context.Pictures on p.PictureId equals v.Id
                           where c.Seasonal_clothes == seasonal_clothes && c.Status == ProductStatus.Active
                           select new
                           {
                               Id = c.Id,
                               Name = c.Name,
                               NormalizedName = c.NormalizedName,
                               Quantity = c.Quantity,
                               Size = c.Size,
                               Color = c.Color,
                               Gender = c.Gender,
                               Price = c.Price,
                               CreatedAt = c.CreatedAt,
                               Seasonal_clothes = c.Seasonal_clothes,
                               Status = c.Status,
                               Description = c.Description,
                               //Picture = v.Filename
                           };

            int i = 0;

            foreach (var item in products)
            {
                if (i != 8)
                {
                    list.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
                    i++;
                }
                else
                    break;
            }

            return list;
        }
        public List<ProductEntity> GetProductsTop10()
        {
            List<ProductEntity> list = new List<ProductEntity>();

            //var products = from p in _context.CategoryProducts
            //               join c in _context.Products on p.ProductId equals c.Id
            //               join v in _context.Pictures on с.PictureId equals v.Id

            var products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           //join v in _context.Pictures on p.PictureId equals v.Id
                           where p.CategoryNormalizedName == "Men Raincoats and windbreakers"  && c.Status == ProductStatus.Active
                           select new
                           {
                               Id = c.Id,
                               Name = c.Name,
                               NormalizedName = c.NormalizedName,
                               Quantity = c.Quantity,
                               Size = c.Size,
                               Color = c.Color,
                               Gender = c.Gender,
                               Price = c.Price,
                               CreatedAt = c.CreatedAt,
                               Seasonal_clothes = c.Seasonal_clothes,
                               Status = c.Status,
                               Description = c.Description,
                               //Picture = v.Filename
                           };

            int i = 0;

            foreach (var item in products)
            {
                if (i != 4)
                {
                    list.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
                    i++;
                }
                else
                    break;
            }

            products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           //join v in _context.Pictures on p.PictureId equals v.Id
                           where p.CategoryNormalizedName == "Men Running shoes" && c.Status == ProductStatus.Active
                           select new
                           {
                               Id = c.Id,
                               Name = c.Name,
                               NormalizedName = c.NormalizedName,
                               Quantity = c.Quantity,
                               Size = c.Size,
                               Color = c.Color,
                               Gender = c.Gender,
                               Price = c.Price,
                               CreatedAt = c.CreatedAt,
                               Seasonal_clothes = c.Seasonal_clothes,
                               Status = c.Status,
                               Description = c.Description,
                               //Picture = v.Filename
                           };

            i = 0;

            foreach (var item in products)
            {
                if (i != 4)
                {
                    list.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
                    i++;
                }
                else
                    break;
            }

            return list;
        }
    }
}
