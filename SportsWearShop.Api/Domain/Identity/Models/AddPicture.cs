using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class AddPicture
    {
        public long Id { get; set; }
        public IFormFile formFiles { get; set; }
    }
}
