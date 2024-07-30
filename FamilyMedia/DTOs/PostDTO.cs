using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public string UFullName { get; set; }
        [Required]
        public string PostTitle { get; set; }
        public System.DateTime PostTime { get; set; }
        [Required]
        public string PostData { get; set; }
        public int LCount { get; set; }
        public int CCount { get; set; }
        [Required]
        public string Privacy { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}