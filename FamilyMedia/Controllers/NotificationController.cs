using FamilyMedia.Auth;
using FamilyMedia.DTOs;
using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace FamilyMedia.Controllers
{
    [Logged]
    public class NotificationController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        // GET: Notification
        public ActionResult Index()
        {
            int userId = (int)Session["UserId"];
            var data = from n in db.Notifications
                       where n.UId == userId
                       orderby n.Date descending
                       select n;
            foreach(var item in data.ToList())
            {
                item.Seen = "Yes";
                db.SaveChanges();
                TempData["Msg"] = "Notification seen";
            }
            return View(Convert(data.ToList()));
        }
        public static NotificationDTO Convert(Notification b)
        {
            return new NotificationDTO()
            {
                NData = b.NData,
                Date = b.Date,
            };
        }
        public static List<NotificationDTO> Convert(List<Notification> notifcations)
        {
            var list = new List<NotificationDTO>();
            foreach (var item in notifcations)
            {
                list.Add(Convert(item));
            }
            return list;
        }
    }
}