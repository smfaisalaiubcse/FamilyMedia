using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public int UId { get; set; }
        public string UFlullName { get; set; }
        [Required]
        public string CommentData { get; set; }
        public System.DateTime CommentTime { get; set; }

        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}