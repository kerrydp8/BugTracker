using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();
        //private UserProfileViewModel userProfile = new UserProfileViewModel();

        // GET: Admin
        public ActionResult UserIndex()
        {

            var roles = db.Roles.ToList();
            var projects = db.Projects;

            var users = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                Email = u.Email
            }).ToList();

            //Listing every user's role and projects on the index
            foreach(var user in users)
            {
                user.CurrentRole = new SelectList(roles, "Name", "Name", roleHelper.ListUserRoles(user.Id).FirstOrDefault());
                user.CurrentProjects = new MultiSelectList(projects, "Id", "Name", projHelper.ListUserProjects(user.Id).Select(p => p.Id));
            }

            return View(users);
        }
        public ActionResult ManageUserRoles(string userId)
        {
            //var currentRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            ViewBag.UserId = userId;
            ViewBag.RoleName = new SelectList(db.Roles.ToList(), "Name", "Name");
            //return View(userProfile);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRoles(string userId, string userRole)
        {
            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            if (! string.IsNullOrEmpty(userId))
            {
                roleHelper.AddUserToRole(userId, userRole);
            }

            //return View();
            return RedirectToAction("UserIndex");
        }

        public ActionResult ManageRoles()
        {
            var users = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                Email = u.Email
            });

            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "FullNameWithEmail");
            ViewBag.RoleName = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View(users);
        }

        [HttpPost]
        public ActionResult ManageRoles(List<string> users, string roleName)
        {
            if (users != null)
            {
                foreach (var userId in users)
                {
                    foreach (var role in roleHelper.ListUserRoles(userId))
                    {
                        roleHelper.RemoveUserFromRole(userId, role);
                    }
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        roleHelper.AddUserToRole(userId, roleName);
                    }
                }
            }

            return RedirectToAction("ManageRoles");
        }

        public ActionResult DeleteUserRole(string userId)
        {
            var roles = roleHelper.ListUserRoles(userId);

            foreach (var role in roles) //We might need to update this so that only one role is removed. We're not quite sure how many roles the user can even have at a time
                                        //but we'll come back to this. 
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            return RedirectToAction("UserIndex");
        }

        public ActionResult ManageProjects(string userId)
        {
            var myProjects = projHelper.ListUserProjects(userId).Select(p => p.Id);
            ViewBag.Projects = new MultiSelectList(db.Projects.ToList(), "Id", "Name", myProjects);
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult ManageProjects(List<int> projects, string userId)
        public ActionResult ManageProjects(List<int> projects, string userId)
        {
            foreach (var project in projHelper.ListUserProjects(userId).ToList())
            {
                projHelper.RemoveUserFromProject(userId, project.Id);
            }
            if (projects != null)
            {
                foreach (var projectId in projects)
                {
                    projHelper.AddUserToProject(userId, projectId);
                }
            }
            return RedirectToAction("UserIndex");    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(int projectId, List<string> ProjectManagers, List<string> Developers, List<string> Submitters)
        {
            //Step 1: Remove all users from the project
            foreach (var user in projHelper.UsersOnProject(projectId).ToList())
            {
                projHelper.RemoveUserFromProject(user.Id, projectId);
            }
            //Step 2: Adds back all the selected PM's
            if (ProjectManagers != null)
            {
                foreach (var projectManagerId in ProjectManagers)
                {
                    projHelper.AddUserToProject(projectManagerId, projectId);
                }
            }
            //Step 3: Adds back all the selected Developers
            if (Developers != null)
            {
                foreach (var developerId in Developers)
                {
                    projHelper.AddUserToProject(developerId, projectId);
                }
            }
            //Step 4: Adds back all the selected Submitters
            if (Submitters != null)
            {
                foreach (var submitterId in Submitters)
                {
                    projHelper.AddUserToProject(submitterId, projectId);
                }
            }
            //Step 4: Redirect the user somewhere
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }

        public ActionResult DeleteUserProjects(string userId)
        {
            var projects = projHelper.ListUserProjects(userId);

            foreach (var proj in projects) //We might need to update this so that only one projects is removed. We're not quite sure how many projects the user can even have at a time
                                        //but we'll come back to this. 
            {
               projHelper.RemoveUserFromProject(userId, proj.Id);
            }

            return RedirectToAction("UserIndex");
        }
    }
}