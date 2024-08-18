using FamilyMedia.Auth;
using FamilyMedia.DTOs;
using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyMedia.Controllers
{
    [Logged]
    public class ProfileController : Controller
    {
        FamilyMediaEntities db = new FamilyMediaEntities();
        public ActionResult MyProfile()
        {
            int Id = (int)Session["UserId"];
            var Profile = (from p in db.Users
                           where p.Id == Id
                           select p).SingleOrDefault();
            int LoggedUserId = (int)Session["UserId"];
            //updated
            var TheyRequestedMe = (from f in db.Friends
                                   join u1 in db.Users on f.FId1 equals u1.Id
                                   where f.FId2 == LoggedUserId
                                   && f.State == "Friend"
                                   select u1).ToList();
            //updated
            var IRequestedThem = (from f in db.Friends
                                  join u1 in db.Users on f.FId2 equals u1.Id
                                  where f.FId1 == LoggedUserId
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
            // Get profile picture
            var profilePicture = (from pp in db.ProfilePictures
                                  where pp.UId == Id
                                  select pp.ImagePath).SingleOrDefault();
            ViewBag.FriendsCount = MyFriends.Count();
            ViewBag.ProfilePicturePath = profilePicture;
            if (Profile == null) return RedirectToAction("ProfileNotFound");
            return View(Convert(Profile));
        }
        public ActionResult Index(int Id)
        {
            var Profile = (from p in db.Users
                           where p.Id == Id
                           select p).SingleOrDefault();
            int LoggedUserId = (int)Session["UserId"];
            //updated
            var TheyRequestedMe = (from f in db.Friends
                                   join u1 in db.Users on f.FId1 equals u1.Id
                                   where f.FId2 == LoggedUserId
                                   && f.State == "Friend"
                                   select u1).ToList();
            //updated
            var IRequestedThem = (from f in db.Friends
                                  join u1 in db.Users on f.FId2 equals u1.Id
                                  where f.FId1 == LoggedUserId
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
            ViewBag.FriendsCount = MyFriends.Count();
            var profilePicture = (from pp in db.ProfilePictures
                                  where pp.UId == Id
                                  select pp.ImagePath).SingleOrDefault();
            ViewBag.ProfilePicturePath = profilePicture;
            return View(Convert(Profile));
        }
        [HttpGet]
        public ActionResult ProfileNotFound()
        {
            return View(); 
        }
        [HttpGet]
        public ActionResult UploadProfilePicture()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadProfilePicture(ProfilePictureDTO image)
        {
            int LoggedUserId = (int)Session["UserId"];

            // Check if a file was uploaded
            if (image.ImageFile == null || image.ImageFile.ContentLength == 0)
            {
                TempData["Msg"] = "Please upload a picture.";
                return RedirectToAction("MyProfile");
            }

            // Get the existing profile picture from the database
            var existingProfilePicture = db.ProfilePictures.SingleOrDefault(pp => pp.UId == LoggedUserId);

            if (existingProfilePicture != null)
            {
                // Remove the existing image file from the Image folder
                string oldImagePath = Server.MapPath(existingProfilePicture.ImagePath);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                // Remove the existing path from the database
                db.ProfilePictures.Remove(existingProfilePicture);
                db.SaveChanges();
            }

            // Save the new image
            string FileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
            string Extension = Path.GetExtension(image.ImageFile.FileName);
            FileName += DateTime.Now.ToString("yymmssfff") + Extension;
            image.ImagePath = "~/Image/" + FileName;
            FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
            image.ImageFile.SaveAs(FileName);

            // Add the new path to the database
            ProfilePicture newProfilePicture = new ProfilePicture
            {
                UId = LoggedUserId,
                ImagePath = image.ImagePath,
            };
            db.ProfilePictures.Add(newProfilePicture);
            db.SaveChanges();

            TempData["Msg"] = "Profile picture updated successfully!";
            return RedirectToAction("MyProfile");
        }

        [HttpPost]
        public ActionResult DeleteProfilePicture()
        {
            int LoggedUserId = (int)Session["UserId"];

            // Get the existing profile picture from the database
            var existingProfilePicture = db.ProfilePictures.SingleOrDefault(pp => pp.UId == LoggedUserId);

            if (existingProfilePicture != null)
            {
                // Remove the existing image file from the Image folder
                string imagePath = Server.MapPath(existingProfilePicture.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                // Remove the profile picture record from the database
                db.ProfilePictures.Remove(existingProfilePicture);
                db.SaveChanges();

                TempData["Msg"] = "Profile picture deleted successfully!";
            }
            else
            {
                TempData["Msg"] = "No profile picture found to delete.";
            }

            return RedirectToAction("MyProfile");
        }

        [HttpGet]
        public ActionResult ChangeProfilePicture()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeProfilePicture(ProfilePictureDTO image)
        {
            int LoggedUserId = (int)Session["UserId"];

            // Check if a file was uploaded
            if (image.ImageFile == null || image.ImageFile.ContentLength == 0)
            {
                TempData["Msg"] = "Please upload a picture.";
                return RedirectToAction("MyProfile");
            }

            // Get the existing profile picture from the database
            var existingProfilePicture = db.ProfilePictures.SingleOrDefault(pp => pp.UId == LoggedUserId);

            if (existingProfilePicture != null)
            {
                // Remove the existing image file from the Image folder
                string oldImagePath = Server.MapPath(existingProfilePicture.ImagePath);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                // Remove the existing path from the database
                db.ProfilePictures.Remove(existingProfilePicture);
                db.SaveChanges();
            }

            // Save the new image
            string FileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
            string Extension = Path.GetExtension(image.ImageFile.FileName);
            FileName += DateTime.Now.ToString("yymmssfff") + Extension;
            image.ImagePath = "~/Image/" + FileName;
            FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
            image.ImageFile.SaveAs(FileName);

            // Add the new path to the database
            ProfilePicture newProfilePicture = new ProfilePicture
            {
                UId = LoggedUserId,
                ImagePath = image.ImagePath,
            };
            db.ProfilePictures.Add(newProfilePicture);
            db.SaveChanges();

            TempData["Msg"] = "Profile picture updated successfully!";
            return RedirectToAction("MyProfile");
        }

        [HttpPost]
        public ActionResult AddFriend(int Id)
        {
            if (Session["UserId"] == null)
            {
                TempData["Msg"] = "You have to login first";
            }
            else if((int)Session["UserId"] == Id)
            {
                TempData["Msg"] = "You can not sent request to your own account xD!!";
            }
            else
            {
                Friend newFriend = new Friend
                {
                    FId1 = (int)Session["UserId"],
                    FId2 = Id,
                    State = "Requested",
                };
                db.Friends.Add(newFriend);
                db.SaveChanges();
                TempData["Msg"] = "Request send successfully";
                return RedirectToAction("Index/" + Id);
            }
            return RedirectToAction("Index/" + Id);
            //return View(); 
        }
        [Logged]
        [HttpGet]
        public ActionResult ViewAllFriends()
        {
            int LoggedUserId = (int)Session["UserId"];

            var TheyRequestedMe = (from f in db.Friends
                                   join u1 in db.Users on f.FId1 equals u1.Id
                                   where f.FId2 == LoggedUserId
                                   && f.State == "Friend"
                                   select u1).ToList();
            //updated
            var IRequestedThem = (from f in db.Friends
                                  join u1 in db.Users on f.FId2 equals u1.Id
                                  where f.FId1 == LoggedUserId
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
            return View(Convert(MyFriends));
        }

        [Logged]
        [HttpGet]
        public ActionResult EditProfile()
        {
            int LoggedUserId = (int)Session["UserId"];
            var Profile = (from p in db.Users
                           where p.Id == LoggedUserId
                           select p).SingleOrDefault();

            if (Profile == null)
            {
                return RedirectToAction("ProfileNotFound");
            }

            var profileDTO = Convert(Profile);
            return View(profileDTO);
        }
        [Logged]
        [HttpPost]
        public ActionResult EditProfile(UserDTO profileDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(profileDTO); // Return the view with validation errors
            }

            int LoggedUserId = (int)Session["UserId"];
            var Profile = (from p in db.Users
                           where p.Id == LoggedUserId
                           select p).SingleOrDefault();

            if (Profile == null)
            {
                return RedirectToAction("ProfileNotFound");
            }

            // Update profile details
            Profile.FullName = profileDTO.FirstName + " " + profileDTO.LastName;
            Profile.Email = profileDTO.Email;
            Profile.Gender = profileDTO.Gender;
            Profile.DOB = profileDTO.DOB;
            Profile.OpeningTime = profileDTO.OpeningTime;
            Profile.SchoolName = profileDTO.SchoolName;
            Profile.CollegeName = profileDTO.CollegeName;
            Profile.UniversityName = profileDTO.UniversityName;
            Profile.WorksAt = profileDTO.WorksAt;

            db.SaveChanges();
            TempData["Msg"] = "Profile updated successfully!";
            return RedirectToAction("Index", new { Id = LoggedUserId });
        }

        public static UserDTO Convert(User u)
        {
            // Split the FullName into parts
            //var nameParts = u.FullName.Split(' ');

            //// Assume the first part is the first name and the last part is the last name
            //string firstName = nameParts.FirstOrDefault();
            //string lastName = nameParts.Length > 1 ? nameParts.Last() : string.Empty;
            string FullName = u.FullName;
            string LastName = "";
            string FirstName = "";
            bool FN = false;
            for (int i = FullName.Length - 1; i >= 0; i--)
            {
                if (FullName[i] == ' ') FN = true;
                if (FN) FirstName += FullName[i];
                else LastName += FullName[i];
            }
            FirstName = new string(FirstName.Reverse().ToArray());
            LastName = new string(LastName.Reverse().ToArray());
            return new UserDTO()
            {
                Id = u.Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = u.Email,
                Gender = u.Gender,
                DOB = u.DOB,
                Password = u.Password,
                OpeningTime = u.OpeningTime,
                SchoolName = u.SchoolName,
                CollegeName = u.CollegeName,
                UniversityName = u.UniversityName,
                WorksAt = u.WorksAt,
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