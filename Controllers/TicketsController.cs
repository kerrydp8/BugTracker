﻿using PagedList;
using PagedList.Mvc;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();
        NotificationHelper nh = new NotificationHelper();

        [Authorize] 
        public ActionResult Dashboard(int? id)
        {
            var ticket = db.Tickets.Find(id);

            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets
        [Authorize]
        public ActionResult Index(int? page, string searchStr)
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketType);

            /*
            ViewBag.Search = searchStr;
            var ticketList = IndexSearch(searchStr);

            int pageSize = 5; // the number of tickets you want to display per page             
            int pageNumber = (page ?? 1);

            return View(ticketList.ToPagedList(pageNumber, pageSize)); //Lists all of the tickets in the order they were created (descending order)
            */

            return View(tickets.ToList());
        }

        /*
        public IQueryable<Ticket> IndexSearch(string searchStr)
        {
            IQueryable<Ticket> result = null;

            if (searchStr != null)
            {
                result = db.Tickets.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr) ||
                p.Description.Contains(searchStr) || p.TicketComments.Any(c => c.CommentBody.Contains(searchStr) ||
                c.User.FirstName.Contains(searchStr) || c.User.LastName.Contains(searchStr) ||
                c.User.DisplayName.Contains(searchStr) || c.User.Email.Contains(searchStr)));
            }
            else
            {
                result = db.Tickets.AsQueryable();
            }

            return result.OrderByDescending(p => p.Created);
        }
        */

        [Authorize(Roles = "Submitter,Developer,Project Manager")]
        public ActionResult MyTickets()
        {
            var userId = User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            //var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketType);

            var myTickets = new List<Ticket>();

            switch (myRole)
            {
                case "Developer":
                    //myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    myTickets = db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
                    break;

                case "Submitter":
                    myTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;

                case "Project Manager":
                    myTickets = db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
                    break;
            }
            return View("Index", myTickets);
            //return View();
        }

        [Authorize(Roles = "Developer")]
        public ActionResult MyAssignments()
        {
            var userId = User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            var myTickets = new List<Ticket>();

            //myTickets = db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
            myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();

            return View("Index", myTickets);
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Administrator, Project Manager, Developer, Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            //var myProjects = projHelper.ListUserProjects(userId);
            ViewBag.ProjectId = new SelectList(db.Users.Find(userId).Projects, "Id", "Name"); //Submitters can only create tickets for projects they are on.
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var localDate = DateTime.UtcNow.AddHours(-4); //Takes extra four hours off so the appropriate time is returned.

                ticket.Created = localDate;
                //ticket.Created = DateTime.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New / Unassigned").Id;

                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "LastName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "LastName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Administrator, Developer, Submitter, Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ticket ticket = db.Tickets.Find(id);
            var allowed = false;
            var userId = User.Identity.GetUserId();

            if (ticket == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Developer") && ticket.AssignedToUserId == userId)
            {
                allowed = true;
            }
            else if (User.IsInRole("Submitter") && ticket.OwnerUserId == userId)
            {
                allowed = true;
            }
            else if (User.IsInRole("Project Manager"))
            {
                allowed = true;
            }
            else
            {
                allowed = true;
            }

            if (allowed)
            {
                ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "LastName", ticket.AssignedToUserId);
                ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "LastName", ticket.OwnerUserId);
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            }
            else
            {
                return RedirectToAction("AccessViolation", "Admin");
            }
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, Project Manager, Developer, Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                var localDate = DateTime.UtcNow.AddHours(-4); //Takes extra four hours off so the appropriate time is returned.

                ticket.Updated = localDate;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                nh.ManageNotifications(oldTicket, ticket);
                HistoryHelper.RecordHistory(oldTicket, ticket);

                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "LastName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "LastName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        [Authorize(Roles = "Submitter")]
        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [Authorize(Roles = "Submitter")]
        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult AssignTicket(int? id)
        {
            UserRolesHelper helper = new UserRolesHelper();
            var ticket = db.Tickets.Find(id);
            var users = helper.UsersInRole("DEVELOPER").ToList();

            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FullNameWithEmail", ticket.AssignedToUserId);
            return View(ticket);
        }

        [Authorize(Roles = "Administrator, Project Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignTicket(Ticket model)
        {
            var ticket = db.Tickets.Find(model.Id);
            var project = db.Projects.Find(ticket.ProjectId);
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
            ticket.AssignedToUserId = model.AssignedToUserId;

            ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New / Assigned").Id; //Automatically updates to Assigned when assigned to dev.
                                                                                                          //The issue was placement. This needed to happen before the changes were saved into the database.
            projHelper.AddUserToProject(ticket.AssignedToUserId, ticket.ProjectId); //Takes in the Id of the current AssignedToUser and the Id of the ticket's Project
            //and addeds the AssignedToUser to the project automatically.

            db.SaveChanges();
            nh.ManageNotifications(oldTicket, ticket);

            var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);

            try
            {
                EmailService ems = new EmailService();
                IdentityMessage msg = new IdentityMessage();
                ApplicationUser user = db.Users.Find(model.AssignedToUserId);

                msg.Body = "You have been assigned a new Ticket." + Environment.NewLine + "Please click the following link to view the details  " + "<a href=\"" + callbackUrl + "\">NEW TICKET</a>";
                msg.Destination = user.Email;
                msg.Subject = "Ticket Assignment";
                await ems.SendMailAsync(msg);
            }

            catch (Exception ex)
            {
                await Task.FromResult(0);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult ClearAssignment(Ticket model)
        {
            var ticket = db.Tickets.Find(model.Id);
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id); //If this ticket's property is changed, a notification will be generated.

            ticket.AssignedToUserId = null; //Upon using this function, the AssignedToUserId for this ticket is removed, essentially "unassigning" the ticket
            ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New / Unassigned").Id; //THe ticket's status is reverted back to Unassigned.

            db.SaveChanges(); //Changes are saved. 
            nh.ManageNotifications(oldTicket, ticket);

            var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);

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
