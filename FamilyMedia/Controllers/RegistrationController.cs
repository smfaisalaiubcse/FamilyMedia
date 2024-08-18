using FamilyMedia.DTOs;
using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyMedia.Controllers
{
    public class RegistrationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        FamilyMediaEntities db = new FamilyMediaEntities();
        [HttpPost]
        public ActionResult Index(UserDTO user)
        {
            try
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    TempData["msg"] = "Email already in use. Please use a different email.";
                    return View("Index");
                }

                db.Users.Add(Convert(user));
                db.SaveChanges();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something went wrong!!" + ex.Message;
            }
            return View(user);
        }
        public static User Convert(UserDTO u)
        {
            return new User
            {
                Id = u.Id,
                Type = "User",
                FullName = u.FirstName + " " + u.LastName,
                Gender = u.Gender,
                DOB = u.DOB,
                Password = u.Password,
                Email = u.Email,
                OpeningTime = DateTime.Now,
            };
        }
    }
}