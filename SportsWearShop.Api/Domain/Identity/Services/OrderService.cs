using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
//using EASendMail;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsWearShop.Api.DataAccess;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;
using System.Net;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public OrderService(IConfiguration configuration, ApiDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }
        private long GetUserId(string login)
        {
            var resultProduct = from t in _context.Users
                                where t.Email == login
                                select t;


            long Id = 0;

            foreach (ApplicationUser item in resultProduct)
            {
                Id = item.Id;
            }

            return Id;
        }
        public void SendPassConfirmOrder(string login)
        {
            SmtpClient Client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "lingprivate1@gmail.com",
                    Password = "Qazwsx1234@"
                }
            };

            MailAddress FromEmail = new MailAddress("lingprivate1@gmail.com", "Ling");
            MailAddress ToEmail = new MailAddress(login, "User");
            MailMessage Message = new MailMessage()
            {
                From = FromEmail,
                Subject = "Перевірка замвлення",
                Body = "Ваш пароль: " + Moderator.RandomGeneratorPass(),
            };

            Message.To.Add(ToEmail);
            Client.SendMailAsync(Message);
        }
        public async Task<string> Create(CreateOrder request)
        {
            var entity = new OrdersEntity
            {
                ProductId = request.ProductId,
                Status_order = Status.Not_active,
                UserId = GetUserId(request.UserLogin),
                Residence = request.Residence,
                Departament = request.Departament,
                Type_of_payment = request.Type_of_payment,
                Phone = request.Phone,
                Email = request.Email,
                CreatedAt = DateTime.Now
            };

            await _context.Orders.AddAsync(entity);

            var result = from t in _context.Products
                         where t.Id == request.ProductId
                         select t;

            foreach (ProductEntity item in result)
            {
                item.Status = Status.Not_active;
                item.Reason = "Додано в замовлення !";
            }
            
            try
            {
                await _context.SaveChangesAsync();

                return "Замовлення успішно додане !";
            }
            catch (Exception)
            {
                return "Невідома помилка";
            }
        }
        public bool CheckPassOrder(LoginDto request)
        {
            if (request.Password == Moderator.TempPass)
                return true;
            else
                return false;
        }
        public List<ProductEntity> GetOrders(string login, string status)
        {
            long UserId = GetUserId(login);

            var products = from b in _context.Orders
                           join p in _context.Products on b.ProductId equals p.Id
                           join u in _context.Users on b.UserId equals u.Id
                           where u.Id == UserId && b.Status_order == status
                           select new
                           {
                               Id = p.Id,
                               Name = p.Name,
                               NormalizedName = p.NormalizedName,
                               Quantity = p.Quantity,
                               Size = p.Size,
                               Color = p.Color,
                               Gender = p.Gender,
                               Price = p.Price,
                               CreatedAt = p.CreatedAt,
                               Seasonal_clothes = p.Seasonal_clothes,
                               Status = p.Status,
                               Description = p.Description,
                               //Picture = v.Filename
                           };

            var list = new List<ProductEntity>();

            foreach (var item in products)
            {
                list.Add(new ProductEntity(item.Id, item.Name, item.NormalizedName, item.Price, item.Description, item.Size, item.Quantity, item.Seasonal_clothes, item.Color, item.Gender, DateTime.Parse(item.CreatedAt.ToString()), item.Status, new FileDto()));
            }

            return list;
        }
    }
}
