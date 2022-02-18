using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.DataAccess.Entities
{
    public class BasketEntity : BaseEntity
    {
        public long? ProductId { get; set; }
        public ProductEntity product { get; set; }
        public long? UserId { get; set; }
        public ApplicationUser user { get; set; }
        public string Status_deleted { get; set; }

        public BasketEntity()
        {

        }

        public BasketEntity(long Id)
        {
            this.Id = Id;            
        }
    }
}
