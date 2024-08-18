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
                //if (TempData["msg"] ==  null)
                //    TempData["Msg"] = "All Notifications are seen now.";
            }
            return View(Convert(data.ToList()));
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var notification = db.Notifications.Find(id);
            if (notification != null)
            {
                db.Notifications.Remove(notification);
                db.SaveChanges();
                TempData["Msg"] = "Notification deleted successfully";
            }
            else
            {
                TempData["Msg"] = "Notification not found.";
            }
            return RedirectToAction("Index");
        }
        public static NotificationDTO Convert(Notification n)
        {
            return new NotificationDTO()
            {
                Id = n.Id,
                NData = n.NData,
                Date = n.Date,
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