using SportsWearShop.Api.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public interface IHomeService
    {
        List<ProductEntity> GetProductsSeasonalClothes(string seasonal_clothes);
        List<ProductEntity> GetProductsTop10();
    }
}
