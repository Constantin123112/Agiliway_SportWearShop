using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;

namespace SportsWearShop.Api.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Birth { get; set; }
        public string Patronymic { get; set; }

        public DateTime? CreatedAt { get; set; }
        public ApplicationUser()
        {

        }

        public ApplicationUser(long Id)
        {
            this.Id = Id;
        }
        public ApplicationUser(long id, string firstname, string surname, string lastname, string gender, string birth, string patronymic, string email)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Surname = surname;
            Gender = gender;
            Birth = birth;
            Patronymic = patronymic;
            Email = email;
        }
    }
}