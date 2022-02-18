using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.DataAccess.Entities
{
    public class OrdersEntity : BaseEntity
    {
        public long? ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public long? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Residence { get; set; }
        public string Departament { get; set; }
        public string Type_of_payment { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status_order { get; set; } // потрібно позвонити замовнику { Active, Not_active}
    }
}
