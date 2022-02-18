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
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add")]
        public IActionResult CreateOrder([FromBody] CreateOrder order)
        {
            var result = _orderService.Create(order);

            return Ok(result);
        }

        [HttpPost("send_pass_confirm_order")]
        public IActionResult SendPassConfirmOrder(string login)
        {
           _orderService.SendPassConfirmOrder(login);

            return Ok();
        }

        [HttpPost("check_pass_order")]
        public IActionResult CheckPassOrder(LoginDto loginDto)
        {
            _orderService.CheckPassOrder(loginDto);

            return Ok();
        }
    }
}
