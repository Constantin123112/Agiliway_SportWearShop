using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class Moderator : Role
    {
        public static string RandomGeneratorPass()
        {
            Random rnd = new Random();
            StringBuilder str = new StringBuilder(8);
            //несколько символов: #-35, $-36, %-37, &-38, @-64
            List<int> Symbols = new List<int> { 35, 36, 37, 38, 64 };
            str.Append((rnd.Next(48, 59)).ToString() + (char)rnd.Next(65, 92) + (char)rnd.Next(97, 124) + (char)Symbols[rnd.Next(0, 5)] + (char)rnd.Next(48, 124) + (char)rnd.Next(97, 124) + (char)rnd.Next(97, 124) + (char)Symbols[rnd.Next(0, 5)]);

            TempPass = str.ToString();

            return str.ToString();
        }
    }
}
