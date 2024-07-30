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
    public class LikeController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        // GET: Like
        //public ActionResult Index()
        //{
        //    return View();
        //}
        [HttpPost]
        public ActionResult Index(int id)
        {
            int UId = (int)Session["UserId"];
            var AlreadyLiked = (from L in db.Likes
                                where L.UId == UId &&
                                L.PostId == id
                                select L).SingleOrDefault();
            if (AlreadyLiked == null)
            {
                Like newLike = new Like
                {
                    PostId = id,
                    UId = (int)Session["UserID"],
                };
                db.Likes.Add(newLike);
                db.SaveChanges();
                var existingPost = db.Posts.Find(id);
                if (existingPost != null)
                {
                    var likers = (from L in db.Likes
                                  where L.PostId == id
                                  select L);
                    ViewBag.LikeCount = likers.Count();
                    existingPost.LCount = likers.Count();
                    db.SaveChanges();
                    TempData["Msg"] = "Like count updated successfully.";
                }
                else
                {
                    TempData["Msg"] = "Post not found.";
                }
                TempData["Msg"] = "You successfully liked this post";
                //return RedirectToAction("ViewAllComment/" + id, "User");
                return RedirectToAction("Index", "User");
            }
            else
            {
                var existingPost = db.Posts.Find(id);
                if (existingPost != null)
                {
                    var likers = (from L in db.Likes
                                  where L.PostId == id
                                  select L);
                    ViewBag.LikeCount = likers.Count();
                    existingPost.LCount = likers.Count();
                    db.SaveChanges();
                    TempData["Msg"] = "Like count updated successfully.";
                }
                TempData["Msg"] = "You already liked this post previously!!!";
                //return RedirectToAction("ViewAllComment/" + id, "User");
                return RedirectToAction("Index", "User");
            }
            //return RedirectToAction("ViewAllComment/" + id, "User");
            return RedirectToAction("Index", "User");
        }
    }
}