using SportsWearShop.Api.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public interface IOrderService
    {
        Task<string> Create(CreateOrder request);
        void SendPassConfirmOrder(string login);
        bool CheckPassOrder(LoginDto request);
    }
}
