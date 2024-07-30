using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string ChatData { get; set; }

        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}