using FamilyMedia.DTOs;
using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyMedia.Controllers
{
    public class LoginController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginDTO l)
        {
            if (ModelState.IsValid)
            {
                var user = (from u in db.Users
                            where u.Email.Equals(l.Email) &&
                            u.Password.Equals(l.Password)
                            select u).SingleOrDefault();
                if (user == null)
                {
                    TempData["Msg"] = "User not found / Uname pass mismatch";
                    return RedirectToAction("Index");
                }
                Session["user"] = user;
                Session["UserId"] = user.Id;
                TempData["userId"] = user.Id;
                Session["userFullName"] = user.FullName;
                return RedirectToAction("Index", "User");
                int UserId = user.Id;
                var Notifications = (from n in db.Notifications
                                     where n.UId == UserId where n.Seen == "No"
                                     select n).ToList();
                int NotificationCount = Notifications.Count();
                Session["NotificationCount"] = NotificationCount;
                TempData["NotificationCount"] = NotificationCount;
                TempData["Msg"] = "Welcome back" + user.FullName;
            }
            ViewBag.userId = TempData["userId"];
            return View(l);
        }
    }
}