using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.DataAccess.Entities
{
    public class PictureUserEntity : BaseEntity
    {
        public string Filename { get; set; }
        public long? UserId { get; set; }
        public ApplicationUser user { get; set; }

        public PictureUserEntity()
        {

        }

        public PictureUserEntity(long Id)
        {
            this.Id = Id;
        }
    }

}
