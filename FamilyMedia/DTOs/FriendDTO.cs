using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMedia.DTOs
{
    public class FriendDTO
    {
        public int Id { get; set; }
        public int FId1 { get; set; }
        public int FId2 { get; set; }
        public string State { get; set; }

        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}