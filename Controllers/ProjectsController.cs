using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace DG_BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projHelper = new ProjectHelper();


        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        //[Authorize]
        public ActionResult MyIndex()
        {
            return View("Index", projHelper.ListUserProjects(User.Identity.GetUserId()).ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            //give details view a multiselectlist of available people per role
            var allPMs = roleHelper.UsersInRole("Project Manager");
            var allDevs = roleHelper.UsersInRole("Developer");
            var allSubmitters = roleHelper.UsersInRole("Submitter");

            //get all current assigned team members
            var assignedPMs = projHelper.UsersInRoleOnProject(project.Id, "Project Manager");
            var assignedDevs = projHelper.UsersInRoleOnProject(project.Id, "Developer");
            var assignedSubmitters = projHelper.UsersInRoleOnProject(project.Id, "Submitter");

            //setting view bag to contain multiselect lists of all members of each specific role
            //maybe make PM a drop down list since only 1 can be on a project at a time?
            ViewBag.ProjectManagers = new MultiSelectList(allPMs, "Id", "FullNameWithEmail", assignedPMs);
            ViewBag.Developers = new MultiSelectList(allDevs, "Id", "FullNameWithEmail", assignedDevs);
            ViewBag.Submitters = new MultiSelectList(allSubmitters, "Id", "FullNameWithEmail", assignedSubmitters);

            return View(project);
        }

        // GET: Projects/Create
        //[Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        //[Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrator, Project Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        //[Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}