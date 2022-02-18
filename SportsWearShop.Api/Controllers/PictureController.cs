using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsWearShop.Api.DataAccess;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;
using SportsWearShop.Api.Domain.Identity.Services;

namespace SportsWearShop.Api.Controllers
{
    [ApiController]
    [Route("picture")]
    public class PictureController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ApiDbContext _context;

        public PictureController(IFileService fileService, ApiDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> Get(string filename)
        {
            var result = await _fileService.Download(filename);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(IFormFile formFile)
        {
            var result = await _fileService.Upload(formFile);

            return Ok(new { result });
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetPicturesById(long id)
        {
            var result = await _fileService.DownloadById(id);

            return Ok(new { result }); 
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(long id)
        {
            var result = await _fileService.DeletePictureById(id);

            return Ok(new { result });
        }

        [HttpGet("getByUserId")]
        public async Task<IActionResult> GetPicturesByUserId(long id)
        {
            var result = await _fileService.DownloadByUserId(id);

            return Ok(new { result });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPicturesToProduct([FromForm] AddPicture picture)
        {
            if (picture.formFiles == null)
                return BadRequest();

            var filenames = await _fileService.BulkUpload(picture.formFiles);

            if (!filenames.Any()) return Ok();
            
            foreach (var filename in filenames)
            {
                await _context.Pictures.AddAsync(new PictureEntity
                {
                    Filename = filename,
                    CreatedAt = System.DateTime.Now,
                    ProductId = picture.Id
                });
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


    }

}