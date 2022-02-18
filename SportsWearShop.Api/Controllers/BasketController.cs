using Microsoft.AspNetCore.Mvc;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;
using SportsWearShop.Api.Domain.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Controllers
{
    [ApiController]
    [Route("basket")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("put")]
        public async Task<IActionResult> Create([FromForm] CreateBasket request)
        {
            var result = await _basketService.Create(request);

            return Ok(new { id = result });
        }
        [HttpGet("checkAddToBasket")]
        public IActionResult Check(string login, long productId)
        {
            var result = _basketService.Check(login, productId);

            return Ok(result);
        }

        [HttpGet("getProducts")]
        public  IActionResult GetProducts(string login)
        {
            var result = _basketService.GetProducts(login);

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string login, long ProductId)
        {
            var result = await _basketService.Delete(login, ProductId);

            return Ok(result);
        }
    }
}
