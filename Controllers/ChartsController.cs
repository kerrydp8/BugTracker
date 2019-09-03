using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class ChartsController: Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public JsonResult GetMorrisData()
        {
            var dataSet = new List<MorrisBarChartData>();

            foreach(var ticketStatus in db.TicketStatuses.ToList())
            {
                var value = db.TicketStatuses.Find(ticketStatus.Id).Tickets.Count();
                dataSet.Add(new MorrisBarChartData { label = ticketStatus.Name, value = value });
            }

            return Json(dataSet);
        }
    }
}