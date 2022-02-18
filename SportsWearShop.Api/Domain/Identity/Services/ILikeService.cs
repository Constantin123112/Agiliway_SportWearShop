using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public interface ILikeService
    {
        Task<string> Create(CreateLike request);
        List<ProductEntity> GetProducts(string login);
        Task<string> Delete(string login, long ProductId);
        bool Check(string login, long productId);
    }
}
