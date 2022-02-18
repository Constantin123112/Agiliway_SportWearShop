using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SportsWearShop.Api.Domain.Identity.Models;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public interface IFileService
    {
        Task<string> Upload(IFormFile formFile);
        Task<List<string>> BulkUpload(IFormFile formFileCollection);
        Task<FileDto> Download(string filename);
        Task<List<FileDto>> DownloadById(long id);
        Task<List<FileDto>> DownloadByUserId(long id);
        Task<string> DeletePictureById(long id);
    }
}