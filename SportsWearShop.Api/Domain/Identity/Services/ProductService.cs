using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportsWearShop.Api.DataAccess;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public class ProductService : IProductService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public ProductService(IConfiguration configuration, ApiDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
            _configuration = configuration;
        }

        private async Task<long> AddProduct(CreateProductDto request)
        {
            var entity = new ProductEntity
            {
                Description = request.Description,
                CreatedAt = System.DateTime.Now,
                Name = request.Name,
                Price = request.Price,
                Color = request.Color,
                Gender = request.Gender,
                Seasonal_clothes = request.Seasonal_clothes,
                NormalizedName = request.NormalizedName,
                Status = ProductStatus.Not_active,
                Reason = "Немає достатньої кількості зображень !"
            };

            var resultEntityProductId = await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();

            return resultEntityProductId.Entity.Id;
        }

        public async Task<string> EditStatusProduct(long Id)
        {
            var resultProduct = from t in _context.Products
                                where t.Id == Id
                                select t;

            foreach (ProductEntity item in resultProduct)
            {
                item.Status = ProductStatus.Active;                
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
        private async Task<long> AddProductSize(CreateProductDto request)
        {
            var entityProductSize = new ProductSizeEntity
            {
                Size = request.Size,
                Count = request.Count
            };

            var resultEntityProductSizeId = await _context.ProductSize.AddAsync(entityProductSize);
            await _context.SaveChangesAsync();

            return resultEntityProductSizeId.Entity.Id;
        }
        private async Task<long> AddProductPicture(CreateProductDto request, long Id)
        {
            if (request.formFileCollection == null)
                return 0;

            var filenames = await BulkUpload2(request.formFileCollection);

            if (!filenames.Any()) return 0;

            foreach (var filename in filenames)
            {
                await _context.Pictures.AddAsync(new PictureEntity
                {
                    Filename = filename,
                    CreatedAt = System.DateTime.Now,
                    ProductId = Id
                });
            }

            try
            {
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<string> Upload(IFormFile formFile)
        {
            if (formFile.Length <= 0) throw new Exception("File invalid");

            var filename = $"{Guid.NewGuid()}_{formFile.FileName}";
            var filePath = Path.Combine(_configuration["BasePicturePath"], filename);

            await using var fs = File.Create(filePath);
            await formFile.CopyToAsync(fs);

            return filename;
        }
        public async Task<List<string>> BulkUpload2(IFormFileCollection formFileCollection)
        {
            var filenames = new List<string>();

            foreach (var file in formFileCollection)
            {
                var filename = await Upload(file);
                filenames.Add(filename);
            }

            return filenames;
        }
        public async Task<long> Create(CreateProductDto request)
        {
            var resProductId = await AddProduct(request);
            //var resProductSizeId = await AddProductSize(request);
            //var resProductPictureId = await AddProductPicture(request, resProductId);

            var Categories = await _context.Categories.FirstOrDefaultAsync(x => x.NormalizedName == request.CategoryNormalizedName);

            await _context.CategoryProducts.AddAsync(new CategoryProductEntity
            {
                CategoryNormalizedName = request.CategoryNormalizedName,
                CategoryId = Categories.Id,
                CreatedAt = System.DateTime.Now,

                ProductId = resProductId,
                //ProductSizeId = resProductSizeId,
                //PictureId = resProductPictureId
            });

            await _context.SaveChangesAsync();

            return resProductId;
        }
        public async Task<long> AddProductSize2(ProductSizeEntity request)
        {

            var entityProductSize = new ProductSizeEntity
            {
                Size = request.Size,
                Count = request.Count,
                ProductId = request.ProductId
            };

            var resultEntityProductSizeId = await _context.ProductSize.AddAsync(entityProductSize);
            await _context.SaveChangesAsync();

            return resultEntityProductSizeId.Entity.Id;
        }
        public async Task<string> Update(ProductEntity product)
        {
            var result = from t in _context.Products
                         where t.Id == product.Id
                         select t;

            foreach (ProductEntity item in result)
            {
                item.Status = product.Status;
                item.Reason = product.Reason;
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
        public List<ProductEntity> ChengeSortBar(SortBar SortBar)
        {
            //var temp = await _context.CategoryProducts.Where(x => x.CategoryNormalizedName == "Men Cross").ToListAsync();
             
            if (SortBar.Price1 == "all" && SortBar.Price2 == "all" && SortBar.Size == "all" && SortBar.Gender == "all" && SortBar.Color == "all" && SortBar.KindOfSport == "all" && SortBar.Seasonal_clothes == "all")
            {
                SortBar.Price1_ = SortBar.Price1;
                SortBar.Price2_ = SortBar.Price2;
                SortBar.Size_ = SortBar.Size;
                SortBar.Gender_ = SortBar.Gender;
                SortBar.Color_ = SortBar.Color;
                SortBar.KindOfSport_ = SortBar.KindOfSport;
                SortBar.Seasonal_clothes_ = SortBar.Seasonal_clothes;
            }
            else
            {
                if (SortBar.Price1 != "all")
                    SortBar.Price1_ = SortBar.Price1;

                if (SortBar.Price2 != "all")
                    SortBar.Price2_ = SortBar.Price2;

                if (SortBar.Size != "all")
                    SortBar.Size_ = SortBar.Size;

                if (SortBar.Gender != "all")
                    SortBar.Gender_ = SortBar.Gender;

                if (SortBar.Categories != "all")
                    SortBar.Categories_ = SortBar.Categories;

                if (SortBar.Color != "all")
                    SortBar.Color_ = SortBar.Color;

                if (SortBar.KindOfSport != "all")
                    SortBar.KindOfSport_ = SortBar.KindOfSport;

                if (SortBar.CategoryNormalizedName != "all")
                    SortBar.CategoryNormalizedName_ = SortBar.CategoryNormalizedName;

                if (SortBar.Seasonal_clothes != "all")
                    SortBar.Seasonal_clothes_ = SortBar.Seasonal_clothes;
            }

            List<ProductEntity> listProducts = new List<ProductEntity>();
            List<FileDto> listPictures = new List<FileDto>();

            
            var products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           where p.CategoryNormalizedName == SortBar.CategoryNormalizedName && c.Status == Status.Active
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
                               Description = c.Description,
                               Status = c.Status
                           };


            if (SortBar.CategoryNormalizedName == "Men" || SortBar.CategoryNormalizedName == "Woman")
            {
                products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           where EF.Functions.Like(p.CategoryNormalizedName, SortBar.CategoryNormalizedName + "%") && c.Status == Status.Active
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
                               Description = c.Description,
                               Status = c.Status
                           };
            }

            List<ProductEntity> listTemp = new List<ProductEntity>();

            foreach (var item in products)
            {
                listProducts.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
            }



            if (SortBar.Price1_ != "all" && SortBar.Price2_ != "all")
            {
                listTemp = listProducts;
                listProducts = new List<ProductEntity>();
                foreach (var item in listTemp)
                {
                    if (Convert.ToInt32(SortBar.Price1_) <= item.Price && Convert.ToInt32(SortBar.Price2_) >= item.Price)
                        listProducts.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
                }

                listTemp = null;
            }

            if (SortBar.Color_ != "all")
            {
                listTemp = listProducts;
                listProducts = new List<ProductEntity>();
                foreach (var item in listTemp)
                {
                    if (item.Color == SortBar.Color_)
                        listProducts.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
                }

                listTemp = null;
            }
            if (SortBar.Size_ != "all")
            {
                listTemp = listProducts;
                listProducts = new List<ProductEntity>();
                foreach (var item in listTemp)
                {
                    if (item.Size == SortBar.Size_)
                        listProducts.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
                }

                listTemp = null;
            }
            if (SortBar.Gender_ != "all")
            {
                listTemp = listProducts;
                listProducts = new List<ProductEntity>();
                foreach (var item in listTemp)
                {
                    if (item.Gender == SortBar.Gender_)
                        listProducts.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
                }

                listTemp = null;
            }
            if (SortBar.Seasonal_clothes_ != "all")
            {
                listTemp = listProducts;
                listProducts = new List<ProductEntity>();
                foreach (var item in listTemp)
                {
                    if (item.Seasonal_clothes == SortBar.Seasonal_clothes_)
                        listProducts.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
                }

                listTemp = null;
            }


            int _i = 0;
            int height = 0;

            for (int i = 0; i < products.Count(); i++)
            {
                if (_i == 4)
                    _i = 0;
                else if (_i == 1)
                    height += 567;
                else
                    _i++;
            }


            return listProducts;
        }

        public ProductEntity GetProductById(long id)
        {
            //var temp = await _context.CategoryProducts.Where(x => x.CategoryNormalizedName == "Men Cross").ToListAsync();

           
            var products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           //join v in _context.Pictures on p.PictureId equals v.Id
                           where c.Id == id
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
                               Description = c.Description,
                               Status = c.Status
                           };


            List<ProductEntity> listTemp = new List<ProductEntity>();

            ProductEntity productEntity = new ProductEntity();

            foreach (var item in products)
            {
                productEntity = new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto());
            }
            
            return productEntity;
        }

        public async Task<List<FileDto>> getPhotos(int id)
        {
            List<FileDto> listPictures = new List<FileDto>();
            var Pictures = await _context.Pictures.Where(x => x.ProductId == id).ToListAsync();

            foreach (var item in Pictures)
            {
                if (item.ProductId == id)
                {
                    var filePath = Path.Combine(_configuration["BasePicturePath"], item.Filename);
                    await using var fs = System.IO.File.OpenRead(filePath);
                    await using var ms = new MemoryStream();
                    await fs.CopyToAsync(ms);
                    var data = ms.ToArray();

                    listPictures.Add(new FileDto(data, item.Filename));
                }                
            }

            return listPictures;
        }

        public async Task<ProductEntity> GetById(long productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);

            return product;
        }

        public string[][] GetListSortBar(string categoryNormalizedName)
        {
            List<SortBar> listSortBar = new List<SortBar>();

            var products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           //join v in _context.Pictures on p.PictureId equals v.Id
                           where p.CategoryNormalizedName == categoryNormalizedName && c.Status == ProductStatus.Active
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
                               Description = c.Description
                           };

            if (categoryNormalizedName == "Men" || categoryNormalizedName == "Woman")
            {
                products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           where EF.Functions.Like(p.CategoryNormalizedName, categoryNormalizedName + "%") && c.Status == Status.Active
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
                               Description = c.Description
                           };
            }


            foreach (var item in products)
            {
                listSortBar.Add(new SortBar(item.Price, item.Size, item.Gender, item.Color, item.Seasonal_clothes));
            }

            var listPricetemp2 = listSortBar.Select(res => res.Price).Distinct().ToList();
            var listSize2 = listSortBar.Select(res => res.Size).Distinct().ToList();
            var listGender2 = listSortBar.Select(res => res.Gender).Distinct().ToList();
            var listColor2 = listSortBar.Select(res => res.Color).Distinct().ToList();
            var listSeasonalClothes2 = listSortBar.Select(res => res.Seasonal_clothes).Distinct().ToList();

            List<string> listPriceSort = new List<string>();
            List<int> listSize3 = new List<int>();

            foreach (var item in listSize2)
            {
                listSize3.Add(Convert.ToInt32(item));
            }

            var listSize = listSize3.OrderBy(c => c).Select(c => c).ToList();
            var listPricetemp = listPricetemp2.OrderBy(x=>x).Select(x=>x).ToList();
            var listGender = listGender2.OrderBy(x => x).Select(x => x).ToList();
            var listColor = listColor2.OrderBy(x => x).Select(x => x).ToList();
            var listSeasonalClothes = listSeasonalClothes2.OrderBy(x => x).Select(x => x).ToList();

            for (int i = 0; i < listPricetemp.Count; i++)
            {
                if (listPricetemp[i] >= 0 && listPricetemp[i] <= 999)
                {
                    listPriceSort.Add("0 - 999");
                }
                else if (listPricetemp[i] >= 1000 && listPricetemp[i] <= 2999)
                {
                    listPriceSort.Add("1000 - 2999");
                }
                else if (listPricetemp[i] >= 3000 && listPricetemp[i] <= 4999)
                {
                    listPriceSort.Add("3000 - 4999");
                }
                else if (listPricetemp[i] >= 5000 && listPricetemp[i] <= 6999)
                {
                    listPriceSort.Add("5000 - 6999");
                }
                else if (listPricetemp[i] >= 7000 && listPricetemp[i] <= 9999)
                {
                    listPriceSort.Add("7000 - 9999");
                }
            }

            var listPrice = listPriceSort.Distinct().ToList();

            string[][] ArrSortBar = new string[5][];
            ArrSortBar[0] = new string[listPrice.Count];
            ArrSortBar[1] = new string[listSize.Count];
            ArrSortBar[2] = new string[listGender.Count];
            ArrSortBar[3] = new string[listColor.Count];
            ArrSortBar[4] = new string[listSeasonalClothes.Count];

            for (int i = 0; i < listPrice.Count; i++)
            {
                ArrSortBar[0][i] = listPrice[i].ToString();
            }

            for (int i = 0; i < listSize.Count; i++)
            {
                ArrSortBar[1][i] = listSize[i].ToString();
            }

            for (int i = 0; i < listGender.Count; i++)
            {
                ArrSortBar[2][i] = listGender[i].ToString();
            }

            for (int i = 0; i < listColor.Count; i++)
            {
                if (listColor[i] == null)
                    continue;
                ArrSortBar[3][i] = listColor[i].ToString();
            }

            for (int i = 0; i < listSeasonalClothes.Count; i++)
            {
                ArrSortBar[4][i] = listSeasonalClothes[i].ToString();
            }

            return ArrSortBar;
        }

        public List<ProductEntity> Search(string search_name)
        {
            List<ProductEntity> list = new List<ProductEntity>();

            //var products = from p in _context.CategoryProducts
            //               join c in _context.Products on p.ProductId equals c.Id
            //               join v in _context.Pictures on с.PictureId equals v.Id

            var products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           //join v in _context.Pictures on p.PictureId equals v.Id
                           where EF.Functions.Like(c.Name, search_name + "%") && c.Status == ProductStatus.Active
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

            foreach (var item in products)
            {
                list.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
            }

            return list;
        }

        public List<ProductEntity> GetHideProducts()
        {
            List<ProductEntity> list = new List<ProductEntity>();

            //var products = from p in _context.CategoryProducts
            //               join c in _context.Products on p.ProductId equals c.Id
            //               join v in _context.Pictures on с.PictureId equals v.Id

            var products = from p in _context.CategoryProducts
                           join c in _context.Products on p.ProductId equals c.Id
                           //join v in _context.Pictures on p.PictureId equals v.Id
                           where c.Status == ProductStatus.Not_active
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
                               Reason = c.Reason
                               //Picture = v.Filename
                           };

            foreach (var item in products)
            {
                list.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto(), item.Reason));
            }

            return list;
        }
        public async Task<string> UpdateProduct(ProductEntity request)
        {
            var resultProduct = from t in _context.Products
                         where t.Id == request.Id
                         select t;

            foreach (ProductEntity item in resultProduct)
            {
                item.Name = request.Name;
                item.Price = request.Price;
                item.Seasonal_clothes = request.Seasonal_clothes;
                item.Color = request.Color;
                item.Gender = request.Gender;
                item.Size = request.Size;
                item.Description = request.Description;
                item.Reason = request.Reason;
            }

            //var resultProductSize = from t in _context.ProductSize
            //                        where t.Id == request.Id
            //                        select t;

            //foreach (ProductSizeEntity item in resultProductSize)
            //{
            //    item.Size = request.Size;
            //    item.Count = request.Quantity;
            //}

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

        public async Task<string> Update2(ProductEntity request)
        {
            var resultProduct = from t in _context.Products
                                where t.Id == request.Id
                                select t;

            foreach (ProductEntity item in resultProduct)
            {
                item.Color = request.Color;
                //item.Gender = request.Gender;
                //item.Size = request.Size;
            }

            //var resultProductSize = from t in _context.ProductSize
            //                        where t.Id == request.Id
            //                        select t;

            //foreach (ProductSizeEntity item in resultProductSize)
            //{
            //    item.Size = request.Size;
            //    item.Count = request.Quantity;
            //}

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