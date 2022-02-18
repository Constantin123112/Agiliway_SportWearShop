using System.Collections.Generic;
using System.Threading.Tasks;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public interface IProductService
    {
        Task<long> Create(CreateProductDto request);
        Task<ProductEntity> GetById(long productId);
        List<ProductEntity> ChengeSortBar(SortBar obj);
        List<ProductEntity> Search(string search_name);
        List<ProductEntity> GetHideProducts();
        string[][] GetListSortBar(string categoryNormalizedName);
        Task<long> AddProductSize2(ProductSizeEntity request);
        Task<string> Update(ProductEntity product);
        Task<string> Update2(ProductEntity product);
        Task<string> EditStatusProduct(long Id);
        ProductEntity GetProductById(long id);
        Task<string> UpdateProduct(ProductEntity request);
    }
}