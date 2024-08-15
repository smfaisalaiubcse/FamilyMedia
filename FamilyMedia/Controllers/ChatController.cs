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