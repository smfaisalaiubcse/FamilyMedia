//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FamilyMedia.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notification
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public System.DateTime Date { get; set; }
        public string NData { get; set; }
        public string Seen { get; set; }
    
        public virtual User User { get; set; }
    }
}