using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportsWearShop.Api.Domain.Identity.Models;
using SportsWearShop.Api.Domain.Identity.Services;
using Microsoft.Extensions.Options;
//using WebApi.Helpers;
using SportsWearShop.Api.DataAccess;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using SportsWearShop.Api.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SportsWearShop.Api.Controllers
{
    public class Person
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    [ApiController]
    [Route("identity")]
    public class IdentityController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;
        private readonly ApiDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IFileService _fileService;


        public IdentityController(IUserService userService, IConfiguration configuration, ApiDbContext context, IWebHostEnvironment hostEnvironment, IFileService fileService)
        {
            _userService = userService;
            _configuration = configuration;
            _context = context;
            _hostEnvironment = hostEnvironment;
            _fileService = fileService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await _userService.Login(request);
            
            return Ok(result);
        }

        [HttpPost("login_admin")]
        public async Task<IActionResult> LoginAdmin(LoginDto request)
        {
            var result = await _userService.LoginAdmin(request);

            return Ok(result);
        }

        [HttpPost("update_pass")]
        public async Task<IActionResult> UpdatePass(LoginDto request)
        {
            var result = await _userService.UpdatePass(request);

            return Ok(result);
        }

        [HttpPost("getUser")]
        public async Task<IActionResult> GetUser(string login)
        {
            var result = await _userService.GetUser(login);

            return Ok(result);
        }

        [HttpPost("getUserById")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var result = await _userService.GetUserById(id);

            return Ok(result);
        }


        [HttpPost("getModerators")]
        public async Task<IActionResult> GetModerators()
        {
            var result = await _userService.GetModerators();

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(long Id)
        {
            var result = await _userService.Delete(new ApplicationUser(Id));

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegistrationDto request)
        {
            var result = await _userService.Register(request);

            if (request.ImageFile == null)
                return BadRequest();

            if (result.Message == "Email already exist")
                return Ok(result);
            if (result.Message == "Invalid password")
                return Ok(result);

            var filenames = await _fileService.BulkUpload(request.ImageFile);

                if (!filenames.Any()) return Ok();

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

                foreach (var filename in filenames)
                {
                    await _context.PictureForUser.AddAsync(new PictureUserEntity
                    {
                        Filename = filename,
                        CreatedAt = System.DateTime.Now,
                        UserId = user.Id
                    });
                }

                await _context.SaveChangesAsync();
            
           
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(ApplicationUser request)
        {
            string msg = await _userService.Update(request);
            
            return Ok(msg);
        }



        [Authorize]
        [HttpGet("auth")]
        public IActionResult Test()
        {
            return Ok($"Авторизован: {User.Identity.IsAuthenticated} {User.Claims.ToList()}");           
        }

        [HttpPost("{userId:long}")]
        public async Task<IActionResult> AddPicturesToProduct(IFormFile formFiles, long userId)
        {
            if (formFiles == null)
                return BadRequest();

            var filenames = await _fileService.BulkUpload(formFiles);

            if (!filenames.Any()) return Ok();

            foreach (var filename in filenames)
            {
                await _context.PictureForUser.AddAsync(new PictureUserEntity
                {
                    Filename = filename,
                    CreatedAt = System.DateTime.Now,
                    UserId = userId
                });
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


        //-----------------------------------------------------------------------------------------------

        /* [HttpGet("getModerators")]
         public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetEmployees()
         {
             return await _context.Users
                 .Select(x => new ApplicationUser()
                 {                    
                     FirstName = x.FirstName,
                     Surname = x.Surname,
                     ImageName = x.Id.ToString(),
                     ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageSrc)
                 })
                 .ToListAsync();
         }

         public async Task<ActionResult<EmployeeModel>> PostEmployeeModel(EmployeeModel employeeModel)
         {
             employeeModel.ImageName = await SaveImage(employeeModel.ImageFile);
             _context.Users.Add(new ApplicationUser());
             await _context.SaveChangesAsync();

             return StatusCode(201);
         }*/

        // GET: api/Employee/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetEmployeeModel(int id)
        {
            var employeeModel = await _context.Users.FindAsync(id);

            if (employeeModel == null)
            {
                return NotFound();
            }

            return employeeModel;
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("ert{id}")]
        public async Task<IActionResult> PutEmployeeModel(int id, EmployeeModel employeeModel)
        {
            if (id != employeeModel.EmployeeID)
            {
                return BadRequest();
            }

            if (employeeModel.ImageFile != null)
            {
                DeleteImage(employeeModel.ImageName);
                employeeModel.ImageName = await SaveImage(employeeModel.ImageFile);
            }

            _context.Entry(employeeModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employee
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> PostEmployeeModel(ApplicationUser employeeModel)
        {
            employeeModel.ImageName = await SaveImage(employeeModel.ImageFile);
            _context.Users.Add(employeeModel);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }*/

        /* public async Task<string> SaveImage(IFormFile imageFile)
         {
             string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
             imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
             var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
             using (var fileStream = new FileStream(imagePath, FileMode.Create))
             {
                 await imageFile.CopyToAsync(fileStream);
             }
             return imageName;
         }*/

        /*[NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }*/
    }
}