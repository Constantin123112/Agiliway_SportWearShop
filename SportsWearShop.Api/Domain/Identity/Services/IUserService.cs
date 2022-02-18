using System.Collections.Generic;
using System.Threading.Tasks;
using SportsWearShop.Api.DataAccess.Entities;
using SportsWearShop.Api.Domain.Identity.Models;

namespace SportsWearShop.Api.Domain.Identity.Services
{
    public interface IUserService
    {
        Task<UserDto> Login(LoginDto request);
        Task<bool> LoginAdmin(LoginDto request);
        Task<string> UpdatePass(LoginDto request);
        Task<UserDto> Register(RegistrationDto request);
        Task<string> Update(ApplicationUser request);
        Task<int> Delete(ApplicationUser request);
        Task<List<UserDto>> GetUser(string login);
        Task<List<UserDto>> GetUserById(long id);
        Task<List<UserDto>> GetModerators();
    }
}