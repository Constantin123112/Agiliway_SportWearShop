using Microsoft.Extensions.Configuration;
using SportsWearShop.Api.DataAccess;
using SportsWearShop.Api.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public class Chart_registration_users
    {
        public string Day { get; set; }
        public int Count { get; set; }

        public Chart_registration_users()
        {

        }

        public Chart_registration_users(string Day, int Count)
        {
            this.Day = Day;
            this.Count = Count;
        }
    }

    public class Chart_added_to_like_products
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public Chart_added_to_like_products()
        {

        }

        public Chart_added_to_like_products(string Name, int Count)
        {
            this.Name = Name;
            this.Count = Count;
        }
    }
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IConfiguration _configuration;
        private readonly ApiDbContext _context;

        public AnalyticsService(IConfiguration configuration, ApiDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public List<Chart_registration_users> Registrations_for_last_month()
        {
            var list = new List<Chart_registration_users>();
            DateTime dateStart = DateTime.Now.AddDays(-10);
           

            var result = from c in _context.Users
                               where c.CreatedAt >= dateStart && c.CreatedAt <= DateTime.Now
                               select c;


            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            int temp = 0;

            for (int i = 0; i < 10; i++)
            {
                temp += 1;
                var day = from c in result
                          where c.CreatedAt == dateTime.AddDays(-temp)
                          select c;

                list.Add(new Chart_registration_users(DateTime.Now.AddDays(-temp).DayOfWeek.ToString(), day.Count()));
            }

            FunctionChart_added_to_like_products();

            return list;
        }

        public List<Chart_added_to_like_products> FunctionChart_added_to_like_products()
        {
            var list = new List<Chart_registration_users>();


            var result = from c in _context.Like                        
                         select c;

            var b = result.Select(res => res.ProductId).Distinct().ToList().Count;

            if (result.Select(res => res.ProductId).Distinct().ToList().Count >= 7)
            {
                long? id = 5;
                long? idTemp = 0;
                int MaxId = 0;
                int maxTemp = 0;

                var res = result.ToList();

                for (int i = 0; i < res.Count; i++)
                {
                    id = res[i].ProductId;
                    MaxId = 0;

                    for (int j = 0; j < res.Count; j++)
                    {
                        if (id == res[j].ProductId)
                            MaxId++;
                    }

                    if (MaxId > maxTemp)
                    {
                        maxTemp = MaxId;
                        idTemp = id;
                    }
                }
            }

            return new List<Chart_added_to_like_products>();
        }
    }
}
