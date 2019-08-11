﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
        public class Ticket
        {
            public int Id { get; set; }

            public int ProjectId { get; set; }
            public int TicketTypeId { get; set; }
            public int TicketPriorityId { get; set; }

            public int TicketStatusId { get; set; }

            public string OwnerUserId { get; set; }
            public string AssignedToUserId { get; set; }

            [StringLength(50, ErrorMessage = "The length of this field must be 5-50 characters long.", MinimumLength = 5)]
            public string Title { get; set; }
            [StringLength(50, ErrorMessage = "The length of this field must be 5-50 characters long.", MinimumLength = 5)]  
            public string Description { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Updated { get; set; }

            //Nav properties
            public virtual Project Project { get; set; }
            public virtual TicketType TicketType { get; set; }
            public virtual TicketStatus TicketStatus { get; set; }
            public virtual ApplicationUser OwnerUser { get; set; }
            public virtual ApplicationUser AssignedToUser { get; set; }

            public virtual ICollection<TicketComment> TicketComments { get; set; }
            public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
            public virtual ICollection<TicketHistory> TicketHistories { get; set; }
            public virtual ICollection<TicketNotification> TicketNotifications { get; set; }

            public Ticket()
            {
                TicketComments = new HashSet<TicketComment>();
                TicketAttachments = new HashSet<TicketAttachment>();
                TicketHistories = new HashSet<TicketHistory>();
                TicketNotifications = new HashSet<TicketNotification>();
            }


    }
}