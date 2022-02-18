using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SportsWearShop.Api.Domain.Identity.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Controllers
{
    [ApiController]
    [Route("LB_24_WEB")]
    public class Controller2 : ControllerBase
    {

        private readonly ILB_24_WEB _lB_24_WEB;

        public Controller2(ILB_24_WEB lB_24_WEB)
        {
            _lB_24_WEB = lB_24_WEB;
        }

        [HttpPost("ZAVD1")]
        public async Task<IActionResult> ZAVD1()
        {
            var result = await _lB_24_WEB.ZAVD1();

            return Ok(result);
        }

        [HttpPost("ZAVD2")]
        public async Task<IActionResult> ZAVD2(int A)
        {
            var result = await _lB_24_WEB.ZAVD2(A);

            return Ok(result);
        }

        [HttpPost("ZAVD3")]
        public async Task<IActionResult> ZAVD3()
        {
            var result = await _lB_24_WEB.ZAVD3();

            return Ok(result);
        }
    }
}
