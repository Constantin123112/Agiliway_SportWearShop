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
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(ApiDbContext context, IJwtGenerator jwtGenerator, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<List<UserDto>> GetUser(string login)
        {
            var moderators = from p in _context.Users
                             join v in _context.PictureForUser on p.Id equals v.UserId
                             where p.Email == login
                             select new
                             {
                                 Id = p.Id,
                                 FirstName = p.FirstName,
                                 Surname = p.Surname,
                                 LastName = p.LastName,
                                 Gender = p.Gender,
                                 Birth = p.Birth,
                                 Patronymic = p.Patronymic,
                                 Email = p.Email,
                                 PictureFileName = v.Filename
                             };

            var listModerators = new List<UserDto>();


            foreach (var item in moderators)
            {
                var filePath = Path.Combine(_configuration["BasePicturePath"], item.PictureFileName);
                await using var fs = System.IO.File.OpenRead(filePath);
                await using var ms = new MemoryStream();
                await fs.CopyToAsync(ms);
                var data = ms.ToArray();

                listModerators.Add(new UserDto(item.Id, item.FirstName, item.Surname, item.LastName, item.Gender, item.Birth, item.Patronymic, item.Email, data));
            }

            return listModerators;
        }

        public async Task<UserDto> Login(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new UserDto("Такого користувача не знайдено !");
            }
			
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded) return new UserDto("Невірний пароль");

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserDto
            {
                Patronymic = user.Patronymic,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Surname = user.Surname,
                Message = "Вхід успішний",
                Gender = user.Gender,
                Roles = roles.ToList(),
                Token = _jwtGenerator.CreateToken(user, roles.ToList())
            };

            if (userDto.Roles[0] == UserRole.Administrator)
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
                MailAddress ToEmail = new MailAddress(request.Email, "User");
                MailMessage Message = new MailMessage()
                {
                    From = FromEmail,
                    Subject = "Перевірка адміністратора",
                    Body = "Ваш пароль: " + Admin.RandomGeneratorPass(),
                };

                Message.To.Add(ToEmail);
                Client.SendCompleted += Client_SendCompleted;
                var t = Client.SendMailAsync(Message);               
               
            }
            else if (userDto.Roles[0] == UserRole.Moderator)
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
                MailAddress ToEmail = new MailAddress(request.Email, "User");
                MailMessage Message = new MailMessage()
                {
                    From = FromEmail,
                    Subject = "Перевірка модератора",
                    Body = "Ваш пароль: " + Moderator.RandomGeneratorPass(),
                };

                Message.To.Add(ToEmail);
                Client.SendCompleted += Client_SendCompleted;
                var t = Client.SendMailAsync(Message);
            }

            return userDto;
        }
      
        private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                string t = e.Error.Message;
                return;
            }

            string temp = "done"; 
        }

        public Task<bool> LoginAdmin(LoginDto request)
        {
            if (request.Password == Admin.TempPass)
                return Task.FromResult(true);
            else
                return Task.FromResult(false);

        }

        public async Task<UserDto> Register(RegistrationDto request)
        {
            if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
            {
                return new UserDto("Email already exist");                
            }

            if (await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync())
            {
                return new UserDto("UserName already exist");
            }

            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Surname = request.Surname,
                Gender = request.Gender,
                Birth = request.Birth,
                Patronymic = request.Patronymic,
                Email = request.Email,
                UserName = request.Email,
                CreatedAt = System.DateTime.Now
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (result.Succeeded)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == newUser.Email);
                
                if (request.Role == UserRole.Buyer)
                    await _userManager.AddToRoleAsync(user, UserRole.Buyer);
                else if (request.Role == UserRole.Moderator)
                    await _userManager.AddToRoleAsync(user, UserRole.Moderator);
                else if (request.Role == UserRole.Administrator)
                    await _userManager.AddToRoleAsync(user, UserRole.Administrator);

                var roles = await _userManager.GetRolesAsync(user);
                
                return new UserDto
                {                    
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Surname = request.Surname,
                    Gender = request.Gender,
                    Patronymic = request.Patronymic,
                    Message = "User adedd",
                    Token = _jwtGenerator.CreateToken(newUser, roles.ToList()),
                    UserName = newUser.UserName,
                    Roles = roles.ToList()
                };
            }
            else
            {
                return new UserDto("Invalid password");
            }


            //return new UserDto("Client creation failed");
        }

        public async Task<string> Update(ApplicationUser request)
        {
            var result = from t in _context.Users
                         where t.Email == request.Email
                         select t;

            foreach (ApplicationUser item in result)
            {
                item.FirstName = request.FirstName;
                item.Surname = request.Surname;
                item.LastName = request.LastName;
                item.Birth = request.Birth;
                item.Gender = request.Gender;
            }

            try
            {
                await _context.SaveChangesAsync();
                return "Дані успішно оновлені !";
            }
            catch (Exception)
            {
                return "Невідома помилка";
            }
        }

        public async Task<string> UpdatePass(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return "Такого користувача не знайдено !";
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded) return "Невірний пароль";

            var resultUpdate = from t in _context.Users
                         where t.Email == request.Email
                         select t;

            //foreach (ApplicationUser item in resultUpdate)
            //{
            //    item.P = request.Password;

            //}
                
            try
            {
                await _context.SaveChangesAsync();
                return "Дані успішно оновлені !";
            }
            catch (Exception)
            {
                return "Невідома помилка";
            }           
        }

        public async Task<List<UserDto>> GetModerators()
        {
            var moderators = from p in _context.Users
                             join c in _context.UserRoles on p.Id equals c.UserId
                             join v in _context.PictureForUser on p.Id equals v.UserId
                             where c.RoleId == 2                             
                             select new
                           {
                               Id = p.Id,
                               FirstName = p.FirstName,
                               Surname = p.Surname,
                               LastName = p.LastName,
                               Gender = p.Gender,
                               Birth = p.Birth,
                               Patronymic = p.Patronymic,
                               Email = p.Email,
                               PictureFileName = v.Filename
                           };
            var listModerators = new List<UserDto>();
            
            

            foreach (var item in moderators)
            {
                var filePath = Path.Combine(_configuration["BasePicturePath"], item.PictureFileName);
                await using var fs = System.IO.File.OpenRead(filePath);
                await using var ms = new MemoryStream();
                await fs.CopyToAsync(ms);
                var data = ms.ToArray();

                listModerators.Add(new UserDto(item.Id, item.FirstName, item.Surname, item.LastName, item.Gender, item.Birth, item.Patronymic, item.Email, data));
            }

            return listModerators;
        }

        public async Task<List<UserDto>> GetUserById(long id)
        {
            var moderators = from p in _context.Users
                             join v in _context.PictureForUser on p.Id equals v.UserId
                             where p.Id == id
                             select new
                             {
                                 Id = p.Id,
                                 FirstName = p.FirstName,
                                 Surname = p.Surname,
                                 LastName = p.LastName,
                                 Gender = p.Gender,
                                 Birth = p.Birth,
                                 Patronymic = p.Patronymic,
                                 Email = p.Email,
                                 PictureFileName = v.Filename
                             };

            var listModerators = new List<UserDto>();


            foreach (var item in moderators)
            {
                var filePath = Path.Combine(_configuration["BasePicturePath"], item.PictureFileName);
                await using var fs = System.IO.File.OpenRead(filePath);
                await using var ms = new MemoryStream();
                await fs.CopyToAsync(ms);
                var data = ms.ToArray();

                listModerators.Add(new UserDto(item.Id, item.FirstName, item.Surname, item.LastName, item.Gender, item.Birth, item.Patronymic, item.Email, data));
            }

            return listModerators;
        }

        public async Task<int> Delete(ApplicationUser request)
        {
            var temp = from p in _context.PictureForUser
                             //join v in _context.PictureForUser on p.Id equals v.UserId
                             where p.UserId == request.Id 
                             select new
                             {
                                 Id = p.Id
                             };

            var picture = new List<PictureUserEntity>();

            foreach (var item in temp)
            {
                picture.Add(new PictureUserEntity(item.Id));
            }

            var result = _context.PictureForUser.Remove(new PictureUserEntity(picture[0].Id)).Entity;

            var result2 = _context.Users.Remove(request).Entity;

           return await _context.SaveChangesAsync();
        }
    }
}