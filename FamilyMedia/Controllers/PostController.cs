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
    public class PostController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult createPost()
        {
            return View();
        }
        [HttpPost]
        public ActionResult createPost(PostDTO blog)
        {
            if (ModelState.IsValid)
            {
                Post newPost = new Post
                {
                    Id = blog.Id,
                    Privacy = blog.Privacy,
                    PostTitle = blog.PostTitle,
                    PostData = blog.PostData,
                    PostTime = DateTime.Now,
                    UId = (int)TempData["UserId"],
                    LCount = 0,
                    CCount = 0,
                    UFullName = (string)Session["userFullName"],
                };
                db.Posts.Add(newPost);
                db.SaveChanges();
                TempData["Msg"] = "Post Successful";
                return RedirectToAction("MyPosts");
            }
            else
            {
                TempData["Msg"] = "Validation Error: " + string.Join(", ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
            }
            return View(blog);
        }

        [HttpGet]
        public ActionResult EditPost(int id)
        {
            var post = db.Posts.Find(id);
            if (post == null)
            {
                TempData["Msg"] = "Post not found.";
                return RedirectToAction("MyPosts");
            }
            return View(Convert(post));
        }

        [HttpPost]
        public ActionResult EditPost(PostDTO post)
        {
            if (ModelState.IsValid)
            {
                var existingPost = db.Posts.Find(post.Id);
                if (existingPost != null)
                {
                    existingPost.PostTitle = post.PostTitle;
                    existingPost.Privacy = post.Privacy;
                    existingPost.PostData = post.PostData;
                    existingPost.PostTime = DateTime.Now;
                    db.SaveChanges();
                    TempData["Msg"] = "Post updated successfully.";
                    return RedirectToAction("MyPosts");
                }
                else
                {
                    TempData["Msg"] = "Post not found.";
                }
            }
            else
            {
                TempData["Msg"] = "Validation Error: " + string.Join(", ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
            }
            return View(post);
        }
        [HttpPost]
        public ActionResult DeletePost(int id)
        {
            var post = db.Posts.Find(id);
            var comments = from c in db.Comments
                           where c.PId == id
                           select c;
            int count = 0;
            if (comments.Any())
            {
                count = 0;
                foreach (var item in comments.ToList())
                {
                    db.Comments.Remove(item);
                    db.SaveChanges();
                    count++;
                }
                //TempData["Msg"] = count + " comments found and Comment deleted successfully.";
            }
            else
            {
                TempData["Msg"] = "Comment not found.";
            }
            var likes = from l in db.Likes
                        where l.PostId == id
                        select l;
            if (likes.Any())
            {
                foreach (var item in likes.ToList())
                {
                    db.Likes.Remove(item);
                    db.SaveChanges();
                }
                //TempData["Msg"] = count + " comments found and Comment deleted successfully.";
            }
            else
            {
                TempData["Msg"] = "No one liked this post";
            }

            if (post != null)
            {
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["Msg"] = "Post deleted successfully with " + count + " comments.";
            }
            else
            {
                TempData["Msg"] = "Post not found.";
            }
            return RedirectToAction("MyPosts");
        }
        [HttpGet]
        public ActionResult MyPosts()
        {
            var Id = (int)HttpContext.Session["userId"];
            var posts = from p in db.Posts
                        where p.UId == Id
                        select p;
            return View(Convert(posts.ToList()));
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
    }
}