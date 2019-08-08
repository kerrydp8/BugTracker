using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectUser
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public ApplicationUser UserId { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public ProjectUser()
        {
            Users = new HashSet<ApplicationUser>();
        }
    }
}