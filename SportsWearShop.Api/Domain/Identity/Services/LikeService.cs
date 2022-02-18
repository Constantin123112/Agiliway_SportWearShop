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
    public class LikeService : ILikeService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public LikeService(IConfiguration configuration, ApiDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }
        private long GetUserId(string login)
        {
            var resultProduct = from t in _context.Users
                                where t.Email == login
                                select t;


            long Id = 0;

            foreach (ApplicationUser item in resultProduct)
            {
                Id = item.Id;
            }

            return Id;
        }

        public async Task<string> Create(CreateLike request)
        {
            var entity = new LikeEntity
            {
                ProductId = request.ProductId,
                Status_deleted = LikeDeleted.Active,
                UserId = GetUserId(request.UserLogin),
                CreatedAt = DateTime.Now
            };

            var resultEntityProductSizeId = await _context.Like.AddAsync(entity);

            try
            {
                await _context.SaveChangesAsync();

                var resultProduct = from t in _context.Products
                                    where t.Id == request.ProductId
                                    select t;


                string productName = null;

                foreach (ProductEntity item in resultProduct)
                {
                    productName = item.Name;
                }

                return $"Продукт {productName} успішно додан в улюблені !";
            }
            catch (Exception)
            {
                return "Невідома помилка";
            }
        }

        public List<ProductEntity> GetProducts(string login)
        {
            long UserId = GetUserId(login);

            var products = from b in _context.Like
                           join p in _context.Products on b.ProductId equals p.Id
                           join u in _context.Users on b.UserId equals u.Id
                           where u.Id == UserId && b.Status_deleted == LikeDeleted.Active
                           select new
                           {
                               Id = p.Id,
                               Name = p.Name,
                               NormalizedName = p.NormalizedName,
                               Quantity = p.Quantity,
                               Size = p.Size,
                               Color = p.Color,
                               Gender = p.Gender,
                               Price = p.Price,
                               CreatedAt = p.CreatedAt,
                               Seasonal_clothes = p.Seasonal_clothes,
                               Status = p.Status,
                               Description = p.Description,
                               //Picture = v.Filename
                           };

            var list = new List<ProductEntity>();

            foreach (var item in products)
            {
                list.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
            }

            return list;
        }

        public bool Check(string login, long productId)
        {
            long UserId = GetUserId(login);

            var products = from b in _context.Like
                           join p in _context.Products on b.ProductId equals p.Id
                           join u in _context.Users on b.UserId equals u.Id
                           where u.Id == UserId && p.Id == productId && b.Status_deleted == LikeDeleted.Active
                           select p;

            var list = new List<ProductEntity>();

            foreach (var item in products)
            {
                list.Add(item);
            }

            if (list.Count > 0)
                return true;

            return false;
        }

        public async Task<string> Delete(string login, long ProductId)
        {

            var t = from b in _context.Like
                    join p in _context.Products on b.ProductId equals p.Id
                    join u in _context.Users on b.UserId equals u.Id
                    where u.Id == GetUserId(login) && p.Id == ProductId
                    select b;



            foreach (LikeEntity item in t)
            {
                item.Status_deleted = LikeDeleted.Not_active;
            }


            try
            {
                await _context.SaveChangesAsync();
                return "Дані успішно оновлені !";
            }
            catch (Exception)
            {
                return "Невідома помилка";
            }
        }
    }
}
