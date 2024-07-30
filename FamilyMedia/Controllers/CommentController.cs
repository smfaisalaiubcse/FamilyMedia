using FamilyMedia.Auth;
using FamilyMedia.DTOs;
using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyMedia.Controllers
{
    [Logged]
    public class CommentController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult createComment(int Id)
        {
            var post = (from p in db.Posts
                        where p.Id == Id
                        select p).SingleOrDefault();
            int posterId = post.UId;
            ViewBag.PostedBy = post.UFullName;
            ViewBag.PostId = post.Id;
            ViewBag.PosterId = post.UId;
            ViewBag.PostTitle = post.PostTitle;
            ViewBag.PostPrivacy = post.Privacy;
            ViewBag.PostData = post.PostData;
            ViewBag.LCount = post.LCount;
            ViewBag.PostTime = post.PostTime;
            var likers = (from L in db.Likes
                          where L.PostId == Id
                          select L);
            ViewBag.LikeCount = likers.Count();
            var comments = from c in db.Comments
                           where c.PId == Id
                           select c;

            var ConvertedComments = Convert(comments.ToList());
            ViewBag.ConvertedComments = ConvertedComments;
            if (post.UId == (int)Session["UserId"]) ViewBag.DeleteAccess = "Ok";
            ViewBag.LoggedUserId = Session["UserId"];
            int LoggedUserId = (int)Session["UserId"];
            var likes = from l in db.Likes
                        where l.UId == LoggedUserId
                        select l;
            ViewBag.Likes = likes.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult createComment(CommentDTO comment, int Id)
        {
            var post = (from p in db.Posts
                        where p.Id == Id
                        select p).SingleOrDefault();
            int posterId = (int)post.UId;
            string postTitle = post.PostTitle;
            ViewBag.PostedBy = post.UFullName;
            ViewBag.PostId = post.Id;
            ViewBag.PosterId = post.UId;
            ViewBag.PostTitle = post.PostTitle;
            ViewBag.PostPrivacy = post.Privacy;
            ViewBag.PostData = post.PostData;
            ViewBag.LCount = post.LCount;
            ViewBag.PostTime = post.PostTime;
            var likers = (from L in db.Likes
                          where L.PostId == Id
                          select L);
            ViewBag.LikeCount = likers.Count();
            ViewBag.LoggedUserId = Session["UserId"];
            int LoggedUserId = (int)Session["UserId"];
            var likes = from l in db.Likes
                        where l.UId == LoggedUserId
                        select l;
            ViewBag.Likes = likes.ToList();
            if (ModelState.IsValid)
            {
                Comment newComment = new Comment
                {
                    Id = comment.Id,
                    PId = Id,
                    UId = (int)Session["UserID"],
                    UFlullName = (string)Session["UserFullName"],
                    CommentTime = DateTime.Now,
                    CommentData = comment.CommentData,
                };
                db.Comments.Add(newComment);
                db.SaveChanges();
                //adding notification to posters id
                if (post.UId != (int)Session["UserID"])
                {
                    Notification newNotification = new Notification
                    {
                        UId = posterId,
                        Date = DateTime.Now,
                        NData = (string)Session["UserFullName"] + " commented on your post, post title: " + postTitle,
                        Seen = "No"
                    };
                    db.Notifications.Add(newNotification);
                    db.SaveChanges();
                    TempData["Msg"] = "Comment Successful";
                    return RedirectToAction("CreateComment/" + Id, "Comment");
                }
            }
            else
            {
                TempData["Msg"] = "Validation Error: " + string.Join(", ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
            }
            var comments = from c in db.Comments
                           where c.PId == Id
                           select c;
            var ConvertedComments = Convert(comments.ToList());
            ViewBag.ConvertedComments = ConvertedComments;
            if (post.UId == (int)Session["UserId"]) ViewBag.DeleteAccess = "Ok";
            ViewBag.LoggedUserId = Session["UserId"];
            return View(comment);
        }

        [HttpGet]
        public ActionResult ViewAllComment(int id)
        {
            var comments = from c in db.Comments
                           where c.PId == id
                           select c;
            var post = (from p in db.Posts
                        where p.Id == id
                        select p).SingleOrDefault();
            var likers = (from L in db.Likes
                          where L.PostId == id
                          select L);
            ViewBag.LikeCount = likers.Count();
            if (post.UId == (int)Session["UserId"]) ViewBag.DeleteAccess = "Ok";
            if (comments.Any())
            {
                ViewBag.LoggedUserId = Session["UserId"];
                ViewBag.PostedBy = post.UFullName;
                ViewBag.PostTitle = post.PostTitle;
                ViewBag.PostData = post.PostData;
                ViewBag.PostTime = post.PostTime;
                return View(Convert(comments.ToList()));
            }

            TempData["Msg"] = "No comments found.";
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public ActionResult EditComment(int id)
        {
            var comment = db.Comments.Find(id);
            if (comment == null)
            {
                TempData["Msg"] = "Comment not found.";
                return RedirectToAction("Index", "User");
            }
            return View(Convert(comment));
        }

        [HttpPost]
        public ActionResult EditComment(CommentDTO comment)
        {
            if (ModelState.IsValid)
            {
                var existingComment = db.Comments.Find(comment.Id);
                int PId = existingComment.PId;
                if (existingComment != null)
                {
                    existingComment.CommentData = comment.CommentData;
                    existingComment.CommentTime = DateTime.Now;
                    db.SaveChanges();
                    TempData["Msg"] = "Comment updated successfully. Post ID: " + existingComment.UId;
                    return RedirectToAction("CreateComment/" + PId, "Comment");
                }
                else
                {
                    TempData["Msg"] = "Comment not found.";
                }
            }
            else
            {
                TempData["Msg"] = "Validation Error: " + string.Join(", ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
            }
            return View(comment);
        }
        [HttpPost]
        public ActionResult DeleteComment(int id)
        {
            var comment = db.Comments.Find(id);
            int PId = comment.PId;
            if (comment != null)
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                TempData["Msg"] = "Comment deleted successfully.";
            }
            else
            {
                TempData["Msg"] = "Comment not found.";
            }
            return RedirectToAction("CreateComment/" + PId, "Comment");
        }
        public static CommentDTO Convert(Comment c)
        {
            return new CommentDTO()
            {
                Id = c.Id,
                PId = c.PId,
                UId = c.UId,
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
    }
}