using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsWearShop.Api.Domain.Identity.Services;
using System.IO;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Controllers
{
   
    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet("get_products_seasonal_clothes")]
        public IActionResult GetProductsSeasonalClothes(string seasonal_clothes)
        {
            var result = _homeService.GetProductsSeasonalClothes(seasonal_clothes);

            return Ok(result);
        }

        [HttpGet("get_products_top_10")]
        public IActionResult GetProductsTop10()
        {
            var result = _homeService.GetProductsTop10();

            return Ok(result);
        }
    }
}