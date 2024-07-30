using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class MeetingDTO
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public string UFullName { get; set; }
        public string MeetingAgenda { get; set; }
        public System.DateTime MeetingStartTime { get; set; }
        public double Duration { get; set; }
        public virtual User User { get; set; }
    }
}