using FamilyMedia.Auth;
using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyMedia.Controllers
{
    [Logged]
    public class FindFriendController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        public ActionResult Index()
        {
            int loggedUserId = (int)Session["UserId"];
            var ProfileEF = (from p in db.Users where p.Id != loggedUserId
                           orderby p.OpeningTime ascending
                           select p);
            var Profiles = Converters.Convert(ProfileEF.ToList());
            //Checking already requested or friend
            var AlreadyFriendOrRequested = from f in db.Friends
                          where f.FId2 == loggedUserId || f.FId1 == loggedUserId
                          select f;
            ViewBag.AlreadyFriendOrRequested = AlreadyFriendOrRequested.ToList();
            //Friend request sent to me
            var FriendRequestEF = from f in db.Friends join u in db.Users on f.FId1 equals u.Id
                                where f.FId2 == loggedUserId &&
                                f.State == "Requested"
                                select u;
            var FriendRequest = Converters.Convert(FriendRequestEF.ToList());
            ViewBag.FriendRequest = FriendRequest;
            ViewBag.FriendRequestCount = FriendRequest.Count();
            Session["FriendRequestCount"] = FriendRequest.Count();
            return View(Profiles);
        }
        [HttpPost]
        public ActionResult Accept(int Id)
        {
            int loggedUserId = (int)Session["UserId"];
            var MakeFriend = (from f in db.Friends
                             where f.FId1 == Id &&
                             f.FId2 == loggedUserId
                             select f).SingleOrDefault();
            if (MakeFriend != null)
            {
                MakeFriend.State = "Friend";
                db.SaveChanges();
                TempData["Msg"] = "Request Accepted";
            }
            return RedirectToAction("Index");
        }
    }
}