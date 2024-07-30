using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public System.DateTime Date { get; set; }
        public string NData { get; set; }
        public string Seen { get; set; }
        public virtual User User { get; set; }
    }
}