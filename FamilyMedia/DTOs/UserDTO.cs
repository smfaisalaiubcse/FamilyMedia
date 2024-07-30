using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class NoSpecialCharactersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string stringValue = value.ToString();
                if (Regex.IsMatch(stringValue, @"^[a-zA-Z\s]+$"))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("The field should not contain special characters or numbers.");
                }
            }
            return ValidationResult.Success;
        }
    }
    public class UserDTO
    {
        public int Id { get; set; }
        [Required]
        [NoSpecialCharacters]
        public string FirstName { get; set; }
        [Required]
        [NoSpecialCharacters]
        public string LastName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Type { get; set; }
        [Required]
        public System.DateTime DOB { get; set; }
        [Required]
        public string Password { get; set; }
        public System.DateTime OpeningTime { get; set; }
        public string SchoolName { get; set; }
        public string CollegeName { get; set; }
        public string UniversityName { get; set; }
        public string WorksAt { get; set; }
        //public IformFile MyProperty { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }
    }
}