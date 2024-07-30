using FamilyMedia.DTOs;
using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMedia.Controllers
{
    public class Converters
    {
        public static UserDTO Convert(User u)
        {
            return new UserDTO()
            {
                Id = u.Id,
                FirstName = u.FullName,
                LastName = u.FullName,
                Email = u.Email,
                Gender = u.Gender,
                DOB = u.DOB,
                Password = u.Password,
                OpeningTime = u.OpeningTime,
                SchoolName = u.SchoolName,
                CollegeName = u.CollegeName,
                UniversityName = u.UniversityName,
                WorksAt = u.WorksAt,
            };
        }
        public static List<UserDTO> Convert(List<User> users)
        {
            var list = new List<UserDTO>();
            foreach (var item in users)
            {
                list.Add(Convert(item));
            }
            return list;
        }
    }
}