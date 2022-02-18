using System;
using System.Collections.Generic;

namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Birth { get; set; }
        public string Patronymic { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public byte[] PictureDate { get; set; } 
        public IList<string> Roles { get; set; }
        public DateTime? CreatedAt { get; set; }


        public UserDto()
        {

        }

        public UserDto(string Message)
        {
            this.Message = Message;
        }
        public UserDto(long id, string firstname, string surname, string lastname, string gender, string birth, string patronymic, string email, byte[] pictureData)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Surname = surname;
            Gender = gender;
            Birth = birth;
            Patronymic = patronymic;
            Email = email;
            PictureDate = pictureData;
        }

        public UserDto(long id, string firstname, string surname, string lastname, string gender, string birth, string patronymic, List<string> Roles)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Surname = surname;
            Gender = gender;
            Birth = birth;
            Patronymic = patronymic;
            this.Roles = Roles;
        }

        public UserDto(long id, string FirstName, string LastName, string Surname, string Gender, string Birth, IList<string> roles)
        {
            Id = id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Surname = Surname;
            this.Gender = Gender;
            this.Birth = Birth;
            Roles = roles;
        }
    }
}