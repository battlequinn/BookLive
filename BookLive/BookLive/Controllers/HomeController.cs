using BookLive.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace BookLive.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db;
        public HomeController()
        {
            db = new ApplicationDbContext();
        }
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
        public ActionResult Browse()
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            List<ApplicationUser> talent = new List<ApplicationUser>();
            foreach(var user in db.Users.ToList())
            {
                var roles = UserManager.GetRoles(user.Id);
                if (roles.Contains("Talent"))
                {
                    talent.Add(user);
                }
            }
            return View(talent);
        }
        
        public ActionResult Notifications()
        {
            var userId = User.Identity.GetUserId();

            List<Request> requests = new List<Request>();

            foreach (var request in db.Request.ToList())
            {
                if (userId == request.ToUserId)
                {
                    requests.Add(request);
                }
            }
            return View(requests);
        }
        
        public ActionResult AcceptOffer()
        {
            return View();
        }

        public ActionResult DeclineOffer()
        {
            return View();
        }

        public ActionResult InitEmail(string talentId)
        {
            return View(new InitEmailViewModel() { TalentId = talentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InitEmail(InitEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            SmtpClient Client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("4097c184a7aa25", "095cde6c9557ea"),
                EnableSsl = true
            };
            MailMessage mailMessage = new MailMessage();
            string subject = "BOOKLIVE: You have a new request!";

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var talent = db.Users.Where(x => x.Id == model.TalentId).FirstOrDefault();

            string body = talent.Email + "," + Environment.NewLine + Environment.NewLine + "You have been requested to play for " + user.Email
                + " at " + model.Place + "!" + Environment.NewLine + Environment.NewLine + "Requested time: " + model.Time + Environment.NewLine
                + "Offer: " + model.Offer + Environment.NewLine + Environment.NewLine + user.Email + "'s Additional Comments: " + model.Comment
                + Environment.NewLine + Environment.NewLine + Environment.NewLine + "Sincerely," + Environment.NewLine + Environment.NewLine
                + "your favorite booking app, BookLive!";

            Client.Send(user.Email, talent.Email , subject, body);

            var request = new Request() {
                ToUserId = talent.Id,
                FromUserId = user.Id,
                FromUserEmail = user.Email,
                Place = model.Place,
                Time = model.Time,
                Offer = model.Offer,
                Comment = model.Comment
            };
            db.Request.Add(request);
            db.SaveChanges();

            return View(model);
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

        public FileContentResult TalentPhotoSamples(string userId)
        {
            if (userId == null)
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
            var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();

            return new FileContentResult(user.UserPhoto, "image/png");
        }

        public FileContentResult UserVideos()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Videos/noVideo.mp4");//Need to fill in video!

                    byte[] videoData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long videoFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    videoData = br.ReadBytes((int)videoFileLength);

                    return File(videoData, "video/mp4");

                }

                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userVideo = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                return new FileContentResult(userVideo.UserVideo, "video/mp4");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Videos/noVideo.mp4");//Need to fill in video!

                byte[] videoData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long videoFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                videoData = br.ReadBytes((int)videoFileLength);

                return File(videoData, "video/mp4");
            }
        }

        public FileContentResult TalentVideoSamples(string userId)
        {
            if (userId == null)
            {
                string fileName = HttpContext.Server.MapPath(@"~/Videos/noVideo.mp4");//Need to fill in video!

                byte[] videoData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long videoFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                videoData = br.ReadBytes((int)videoFileLength);

                return File(videoData, "video/mp4");

            }

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();

            return new FileContentResult(user.UserVideo, "video/mp4");
        }
    }
}