namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                new TicketPriority { Name = "Immediate", Description = "Highest priority level"},
                new TicketPriority { Name = "High", Description = ""},
                new TicketPriority { Name = "Medium"},
                new TicketPriority { Name = "Low" },
                new TicketPriority { Name = "None" }
                );

            context.TicketStatuses.AddOrUpdate(
                t => t.Name,
                new TicketStatus { Name = "New / UnAssigned", Description = ""},
                new TicketStatus { Name = "UnAssigned", Description = "" },
                new TicketStatus { Name = "New / Assigned", Description = "" },
                new TicketStatus { Name = "Assigned", Description = "" },
                new TicketStatus { Name = "In Progress", Description = "" },
                new TicketStatus { Name = "Completed", Description = "" },
                new TicketStatus { Name = "Archived", Description = "" }
                );

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                new TicketType { Name = "Bug / Defect", Description = "" },
                new TicketType { Name = "New Functionality Request", Description = "" },
                new TicketType { Name = "New Document Request", Description = "" },
                new TicketType { Name = "Training Request", Description = "" },
                new TicketType { Name = "Complaint", Description = "" }
                );

            #region roleManager

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {
                roleManager.Create(new IdentityRole
                {
                    Name = "Administrator"
                });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole
                {
                    Name = "Project Manager"
                });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole
                {
                    Name = "Developer"
                });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole
                {
                    Name = "Submitter"
                });
            }
            #endregion

            //I need to create a few users that will eventually occupy the roles of either Admin or Moderator
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "kerrydp8@outlook.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "kerrydp8@outlook.com",
                    Email = "kerrydp8@outlook.com",
                    FirstName = "Kerry",
                    LastName = "Peay",
                    DisplayName = "kerrydp8"
                }, "Wiiugamer12");
            }

            if (!context.Users.Any(u => u.Email == "PM@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "PM@Mailinator.com",
                    Email = "PM@Mailinator.com",
                    FirstName = "Tom",
                    LastName = "Cruise",
                    DisplayName = "TomCruise"
                }, "TomCruise1!");
            }

            if (!context.Users.Any(u => u.Email == "Developer@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer@Mailinator.com",
                    Email = "Developer@Mailinator.com",
                    FirstName = "Dennis",
                    LastName = "Hopper",
                    DisplayName = "DennisHopper"
                }, "DennisHopper2!");
            }

            if (!context.Users.Any(u => u.Email == "Submitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Submitter@Mailinator.com",
                    Email = "Submitter@Mailinator.com",
                    FirstName = "Jessica",
                    LastName = "Chastain",
                    DisplayName = "JessicaChastain"
                }, "JessicaChastain3!");
            }

            var userId = userManager.FindByEmail("kerrydp8@outlook.com").Id;
            userManager.AddToRole(userId, "Administrator");

            userId = userManager.FindByEmail("PM@Mailinator.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            userId = userManager.FindByEmail("Developer@Mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            userId = userManager.FindByEmail("Submitter@Mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");


        }
    }
}
