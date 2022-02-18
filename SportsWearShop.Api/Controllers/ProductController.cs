using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;
using SportsWearShop.Api.Domain.Identity.Services;

namespace SportsWearShop.Api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {   
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto request)
        {
            var result = await _productService.Create(request);

            return Ok( new { id = result });
        }

        [HttpPost("addProduct_size")]
        public async Task<IActionResult> AddProductSize(ProductSizeEntity request)
        {
            var result = await _productService.AddProductSize2(request);

            return Ok(new { id = result });
        }

        [HttpGet]
        public async Task<ProductEntity> GetProductById(long id)
        {
            var result = await _productService.GetById(id);

            return result;
        }

        [HttpPost("ChengeSortBar")]
        public IActionResult ChengeSortBar(SortBar obj)
        {
            var result = _productService.ChengeSortBar(obj);

            return Ok(result);
        }

        [HttpPost("GetListSortBar")]
        public IActionResult GetListSortBar(string categoryNormalizedName)
        {
            var result = _productService.GetListSortBar(categoryNormalizedName);

            return Ok(result);
        }

        [HttpPost("hide_product")]
        public async Task<IActionResult> Update(ProductEntity product)
        {
            string msg = await _productService.Update(product);

            return Ok(msg);
        }

        [HttpGet("search")]
        public IActionResult Search(string name)
        {
            var objs = _productService.Search(name);

            return Ok(objs);
        }

        [HttpGet("get_hide_products")]
        public IActionResult Search()
        {
            var objs = _productService.GetHideProducts();

            return Ok(objs);
        }


        [HttpPost("update")]
        public async Task<IActionResult> Search([FromForm] ProductEntity product)
        {
            var msg = await _productService.UpdateProduct(product);

            return Ok(msg);
        }

        [HttpPost("update_addToBasket")]
        public async Task<IActionResult> UpdateToBasket([FromForm] ProductEntity product)
        {
            var msg = await _productService.Update2(product);

            return Ok(msg);
        }

        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductByid(long id)
        {
            var result = _productService.GetProductById(id);

            return Ok(result);
        }

        [HttpGet("edit_status")]
        public async Task<IActionResult> EditStatusProduct(long Id)
        {
            var result = await _productService.EditStatusProduct(Id);

            return Ok(result);
        }
    }    
}