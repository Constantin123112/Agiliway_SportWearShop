using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.DataAccess.Entities
{
    public class LikeEntity : BaseEntity
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public ProductEntity product { get; set; }
        public long? UserId { get; set; }
        public ApplicationUser user { get; set; }
        public string Status_deleted { get; set; }
    }
}
