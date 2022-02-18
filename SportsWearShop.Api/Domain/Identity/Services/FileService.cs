using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportsWearShop.Api.DataAccess;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public class FileService : IFileService
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration, ApiDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> Upload(IFormFile formFile)
        {
            if (formFile.Length <= 0) throw new Exception("File invalid");
            
            var filename = $"{Guid.NewGuid()}_{formFile.FileName}";
            var filePath = Path.Combine(_configuration["BasePicturePath"], filename);
            
            await using var fs = File.Create(filePath);
            await formFile.CopyToAsync(fs);

            return filename;
        }

        public async Task<List<string>> BulkUpload(IFormFile formFileCollection)
        {
            var filenames = new List<string>();
            
            //foreach (var file in formFileCollection)
            //{
                var filename = await Upload(formFileCollection);
                filenames.Add(filename);
            //}

            return filenames;
        }

        public async Task<List<string>> BulkUpload2(IFormFileCollection formFileCollection)
        {
            var filenames = new List<string>();

            foreach (var file in formFileCollection)
            {
                var filename = await Upload(file);
                filenames.Add(filename);
            }

            return filenames;
        }

        public async Task<string> DeletePictureById(long id)
        {
            var Pictures = _context.Pictures.Remove(new PictureEntity(id));

           await _context.SaveChangesAsync();
            return "Успішно видалено !";
        }

        public async Task<FileDto> Download(string filename)
        {
            var filePath = Path.Combine(_configuration["BasePicturePath"], filename);
            await using var fs = System.IO.File.OpenRead(filePath);
            await using var ms = new MemoryStream();
            await fs.CopyToAsync(ms);
            var data = ms.ToArray();

            return new FileDto
            {
                Data = data,
                Filename = filename
            };
        }

        public async Task<List<FileDto>> DownloadById(long id)
        {
            List<FileDto> listPictures = new List<FileDto>();
            var Pictures = await _context.Pictures.Where(x => x.ProductId == id).ToListAsync();

            foreach (var item in Pictures)
            {
                if (item.ProductId == id)
                {
                    var filePath = Path.Combine(_configuration["BasePicturePath"], item.Filename);
                    await using var fs = System.IO.File.OpenRead(filePath);
                    await using var ms = new MemoryStream();
                    await fs.CopyToAsync(ms);
                    var data = ms.ToArray();

                    listPictures.Add(new FileDto(data, item.Filename, item.Id));
                }
            }

            return listPictures;
        }

        public async Task<List<FileDto>> DownloadByUserId(long id)
        {
            List<FileDto> listPictures = new List<FileDto>();
            var Pictures = await _context.PictureForUser.Where(x => x.UserId == id).ToListAsync();

            foreach (var item in Pictures)
            {
                if (item.UserId == id)
                {
                    var filePath = Path.Combine(_configuration["BasePicturePath"], item.Filename);
                    await using var fs = System.IO.File.OpenRead(filePath);
                    await using var ms = new MemoryStream();
                    await fs.CopyToAsync(ms);
                    var data = ms.ToArray();

                    listPictures.Add(new FileDto(data, item.Filename));
                }
            }

            return listPictures;
        }
    }
}