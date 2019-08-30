using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();
        public ActionResult Index()
        {
            /*
            var user = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                Email = u.Email
            }).ToList();
            */
            //return View(user);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }

        public ActionResult Tables()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var from = model.FromName + "," + $"{model.FromEmail}<{ConfigurationManager.AppSettings["emailto"]}>"; //THe name and address of the person who entered it.

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject, //The subject of the email.
                        Body = model.Body, //The body of the email.
                        IsBodyHtml = true
                    };
                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
            //Console.WriteLine("Message has been sent");
            //return View("Index", "Home"); //Does not work
        }

        public ActionResult MyProjects(string userId)
        {
            //var myProjects = projHelper.ListUserProjects(userId).Select(p => p.Id);
            //ViewBag.Projects = new MultiSelectList(db.Projects.ToList(), "Id", "Name", myProjects);

            var projects = db.Projects.Select(p => new Project
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                //Created = p.Created,
                //Tickets = p.Tickets,
                //Users = p.Users
            });

            return View(projects);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProjects(List<int> projects, string userId)
        {
            foreach (var project in projHelper.ListUserProjects(userId).ToList())
            {
                projHelper.ListUserProjects(userId);
            }
            /*
            if (projects != null)
            {
                foreach (var projectId in projects)
                {
                    projHelper.AddUserToProject(userId, projectId);
                }
            }
            */
            return View();
        }

        public ActionResult DemoUser()
        {
            return View();
        }
    }
}