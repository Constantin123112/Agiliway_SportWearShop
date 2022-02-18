using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.DataAccess.Entities
{
    public class ProductSizeEntity : BaseEntity
    {
        public string Size { get; set; }
        public int Count { get; set; }
        public long? ProductId { get; set; }
        public ProductEntity product { get; set; }
    }
}
