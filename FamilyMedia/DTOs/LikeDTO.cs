using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class LikeDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UId { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}