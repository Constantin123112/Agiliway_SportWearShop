using Microsoft.AspNetCore.Mvc;
using SportsWearShop.Api.Domain.Identity.Models;
using SportsWearShop.Api.Domain.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Controllers
{
    [ApiController]
    [Route("like")]
    public class LikeController : Controller
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("put")]
        public async Task<IActionResult> Create([FromForm] CreateLike request)
        {
            var result = await _likeService.Create(request);

            return Ok (result);
        }

        [HttpGet("getProducts")]
        public IActionResult GetProducts(string login)
        {
            var result = _likeService.GetProducts(login);

            return Ok(result);
        }

        [HttpGet("checkAddToLike")]
        public IActionResult Check(string login, long productId)
        {
            var result = _likeService.Check(login, productId);

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string login, long ProductId)
        {
            var result = await _likeService.Delete(login, ProductId);

            return Ok(result);
        }
    }
}
