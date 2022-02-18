using Microsoft.Extensions.Configuration;
using SportsWearShop.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public class LB_23_WEB : ILB_24_WEB
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public LB_23_WEB(IConfiguration configuration, ApiDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public int[] RandomMasiv(int min, int max, int size)
        {
            int[] Arr = new int[size];

            for (int i = 0; i < Arr.Length; i++)
            {
                Arr[i] = new Random().Next(min, max);
            }

            return Arr;
        }

        public Task<int> ZAVD1()
        {
            int[] Arr = RandomMasiv(-10, 20, 30);
            int sum = 0;

            for (int i = 0; i < Arr.Length; i++)
            {
                if ((i % 2) != 0)
                    sum += Arr[i];
            }

            return Task.FromResult(sum);
        }

        public Task<int> ZAVD2(int A)
        {
            int[] Arr = RandomMasiv(-10, 20, 30);
            int n = 0;

            for (int i = 0; i < Arr.Length; i++)
            {
                if (Arr[i] > A && (Arr[i] % 5) == 0)
                    n++;
            }

            return Task.FromResult(n);
        }

        public Task<int> ZAVD3()
        {
            int[] Arr = RandomMasiv(-30, 10, 30);
            int number = 0;

            for (int i = 0; i < Arr.Length; i++)
            {
                if ((Arr[i] % 5) == 2)
                    number = Arr[i];
            }

            return Task.FromResult(number);
        }

        
    }
}
