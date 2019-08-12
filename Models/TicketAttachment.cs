﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
        public class TicketAttachment
        {
        public int Id { get; set; }
        public int TicketId { get; set; }
        //public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AttachmentUrl { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }

        //Virtual nav
        public virtual Ticket Ticket { get; set; }
        //public virtual ApplicationUser UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}