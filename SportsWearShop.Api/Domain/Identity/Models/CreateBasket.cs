using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class CreateBasket
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? UserId { get; set; }
        public string UserLogin { get; set; }
    }

    public static class BasketDeleted
    {
        public const string Active = "Active";
        public const string Not_active = "Not_active";
    }
}
