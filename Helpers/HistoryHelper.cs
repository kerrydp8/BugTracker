using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Helpers
{
    public class HistoryHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        public static void RecordHistory(Ticket oldTicket, Ticket newTicket)
        {
            foreach (var property in WebConfigurationManager.AppSettings["TrackedHistoryProperties"].Split(','))
            {
                var oldValue = oldTicket.GetType().GetProperty(property).GetValue(oldTicket, null).ToString();
                var newValue = newTicket.GetType().GetProperty(property).GetValue(newTicket, null).ToString();

                if(oldValue != newValue)
                {
                    var newHistory = new TicketHistory
                    {
                        UserId = HttpContext.Current.User.Identity.GetUserId(),
                        Updated = (DateTime)newTicket.Updated,
                        PropertyName = property,
                        OldValue = oldValue,
                        NewValue = newValue,
                        TicketId = newTicket.Id
                    };
                    db.TicketHistories.Add(newHistory);
                }
            }
            db.SaveChanges();
        }
    }
}