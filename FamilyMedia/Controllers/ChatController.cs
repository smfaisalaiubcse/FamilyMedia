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
    public class ChatController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        // GET: Chat
        [HttpGet]
        public ActionResult Index()
        {
            int UserId = (int)Session["UserId"];
            var chat = from c in db.Chats
                       where c.ReceiverId == UserId
                       select c;

            return View(Convert(chat.ToList()));
        }
        //[HttpGet]
        //public ActionResult Index(int Id)
        //{
        //    int UserId = (int)Session["UserId"];
        //    var chat = from c in db.Chats
        //               where c.ReceiverId == UserId
        //               select c;

        //    return View(Convert(chat.ToList()));
        //}
        [HttpGet]
        public ActionResult Create(int Id)
        {
            var receiver = (from r in db.Users
                            where r.Id == Id
                            select r).SingleOrDefault();
            //return View(ProfileController.Convert(receiver));
            return View();
        }
        [HttpPost]
        public ActionResult Create(ChatDTO chat, int Id)
        {
            var receiver = (from r in db.Users
                           where r.Id == Id
                           select r).SingleOrDefault();
            if (receiver == null) return RedirectToAction("ProfileNotFound");
            if (ModelState.IsValid)
            {
                Chat newChat = new Chat
                {
                    Id = chat.Id,
                    SenderId = (int)Session["UserId"],
                    ReceiverId= Id,
                    ChatData = chat.ChatData,
                };
                db.Chats.Add(newChat);
                db.SaveChanges();
                TempData["Msg"] = "Chat started Successful";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Msg"] = "Validation Error: " + string.Join(", ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
            }
            return RedirectToAction("Index");
            //return View(chat);
            //return View(ProfileController.Convert(Profile));
        }
        public static ChatDTO Convert(Chat chat)
        {
            return new ChatDTO()
            {
                Id=chat.Id,
                SenderId=chat.SenderId,
                ReceiverId=chat.ReceiverId,
            };
        }
        public static List<ChatDTO> Convert(List<Chat> chats)
        {
            var list = new List<ChatDTO>();
            foreach (var item in chats)
            {
                list.Add(Convert(item));
            }
            return list;
        }
    }
}