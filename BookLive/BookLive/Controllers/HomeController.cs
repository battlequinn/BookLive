﻿using BookLive.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookLive.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View("../Home/AdminIndex", User);
            }
            else if (User.IsInRole("Managaer"))
            {
                return View("../Home/ManagerIndex", User);
            }
            else if (User.IsInRole("Employee"))
            {
                return View("../Home/EmployeeIndex", User);
            }
            else if (User.IsInRole("Planner"))
            {
                return View("../Home/PlannerIndex", User);
            }
            else if (User.IsInRole("Talent"))
            {
                return View("../Home/TalentIndex", User);
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();

                if(userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");//Need to fill in image!

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }

                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");//Need to fill in image!

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");
            }
        }
    }
}