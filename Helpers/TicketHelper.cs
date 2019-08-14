using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Ticket> TicketsForProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            return project.Tickets.ToList();
        }

        public bool IsTicketForProject(int ticketId, int projectId)
        {

            var userId = HttpContext.Current.User.Identity.GetUserId();

            if (db.Projects.Find(projectId).Tickets.Contains(db.Tickets.Find(ticketId)) && (db.Projects.Find(projectId).Users.Contains(db.Users.Find(userId))))
            {
                return true;
            }
            return false;
        }

    }
}