using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SportsWearShop.Api.DataAccess;
using SportsWearShop.Api.Domain.Identity.Services;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApiDbContext _context;
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IConfiguration configuration, ApiDbContext context, IAnalyticsService analyticsService)
        {           
            _configuration = configuration;
            _context = context;
            _analyticsService = analyticsService;
        }

        [HttpGet("registrations_for_last_month")]
        public IActionResult Analytics1()
        {
            var res = _analyticsService.Registrations_for_last_month();

            return Ok(res);
        }
    }
}
