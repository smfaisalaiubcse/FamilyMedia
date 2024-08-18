using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class ProfilePictureDTO
    {
        public int Id { get; set; }
        public int UId { get; set; }
        [DisplayName("Upload Image")]
        public string ImagePath { get; set; }
        [Required]
        public HttpPostedFileBase ImageFile { get; set; }

        public virtual User User { get; set; }
    }
}