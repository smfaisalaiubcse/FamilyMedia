using FamilyMedia.Auth;
using FamilyMedia.DTOs;
using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace FamilyMedia.Controllers
{
    [Logged]
    public class UserController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        public ActionResult Index()
        {

            var data = from p in db.Posts
                           where p.Privacy != "OnlyMe" orderby p.PostTime descending 
                           select p;
            int UserId = (int)Session["UserId"];
            //Load number of request sent to me
            //Friend request sent to me
            var FriendRequestEF = from f in db.Friends
                                  join u in db.Users on f.FId1 equals u.Id
                                  where f.FId2 == UserId &&
                                  f.State == "Requested"
                                  select u;
            var FriendRequest = Converters.Convert(FriendRequestEF.ToList());
            ViewBag.FriendRequest = FriendRequest;
            Session["FriendRequestCount"] = FriendRequest.Count();
            //updated
            var TheyRequestedMe = (from f in db.Friends
                                   join u1 in db.Users on f.FId1 equals u1.Id
                                   where f.FId2 == UserId
                                   && f.State == "Friend"
                                   select u1).ToList();
            //updated
            var IRequestedThem = (from f in db.Friends
                                  join u1 in db.Users on f.FId2 equals u1.Id
                                  where f.FId1 == UserId
                                  && f.State == "Friend"
                                  select u1).ToList();
            var MyFriends = TheyRequestedMe;
            foreach (var i in IRequestedThem)
            {
                bool ok = true;
                foreach (var j in TheyRequestedMe)
                {
                    if (i == j)
                    {
                        ok = false;
                        continue;
                    }
                }
                if (ok) MyFriends.Add(i);
            }
            List<Post> posts = new List<Post>();
            foreach(var i in data)
            {
                if (i.Privacy == "Friends")
                {
                    foreach (var j in MyFriends)
                    {
                        if (i.UId == j.Id)
                        {
                            posts.Add(i);
                            break;
                        }
                    }
                }
                else
                {
                    if(i.Privacy != "OnlyMe")
                        posts.Add(i);
                }
            }
            var Notifications = (from n in db.Notifications
                                 where n.UId == UserId
                                 where n.Seen == "No"
                                 select n).ToList();
            int NotificationCount = Notifications.Count();
            Session["NotificationCount"] = NotificationCount;
            TempData["NotificationCount"] = NotificationCount;
            var likes = from l in db.Likes
                       where l.UId == UserId
                       select l;
            ViewBag.Likes = likes.ToList();
            return View(Convert(posts.ToList()));
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["UserId"] = null;
            Session["FriendRequestCount"] = null;
            return RedirectToAction("Index", "Home");
        }
        public static MeetingDTO Convert(Meeting m)
        {
            return new MeetingDTO()
            {
                Id = m.Id,
                UId = m.UId,
                UFullName = m.UFullName,
                MeetingAgenda = m.MeetingAgenda,
                MeetingStartTime = m.MeetingStartTime,
                Duration = m.Duration
            };
        }
        public static List<MeetingDTO> Convert(List<Meeting> meetings)
        {
            var list = new List<MeetingDTO>();
            foreach (var item in meetings)
            {
                list.Add(Convert(item));
            }
            return list;
        }
        public static PostDTO Convert(Post p)
        {
            return new PostDTO()
            {
                Id = p.Id,
                Privacy = p.Privacy,
                PostTitle = p.PostTitle,
                PostData = p.PostData,
                PostTime = p.PostTime,
                UId = p.UId,
                LCount = p.LCount,
                CCount = p.CCount,
                UFullName = p.UFullName,
            };
        }
        public static List<PostDTO> Convert(List<Post> blogs)
        {
            var list = new List<PostDTO>();
            foreach (var item in blogs)
            {
                list.Add(Convert(item));
            }
            return list;
        }
        public static CommentDTO Convert(Comment c)
        {
            return new CommentDTO()
            {
                Id = c.Id,
                PId = c.PId,
                UId= c.UId,
                UFlullName = c.UFlullName,
                CommentData = c.CommentData,
                CommentTime = c.CommentTime,
            };
        }
        public static List<CommentDTO> Convert(List<Comment> comments)
        {
            var list = new List<CommentDTO>();
            foreach (var item in comments)
            {
                list.Add(Convert(item));
            }
            return list;
        }
        public static UserDTO Convert(User u)
        {
            return new UserDTO()
            {
                Id = u.Id,
                FirstName = u.FullName,
                LastName = u.FullName,
                Email = u.Email,
                Gender = u.Gender,
                DOB = u.DOB,
                Password = u.Password,
            };
        }
        public static List<UserDTO> Convert(List<User> users)
        {
            var list = new List<UserDTO>();
            foreach (var item in users)
            {
                list.Add(Convert(item));
            }
            return list;
        }
    }
}