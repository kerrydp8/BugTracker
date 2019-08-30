namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

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

            context.TicketTypes.AddOrUpdate(
             t => t.Name,
                new TicketType { Id = 100, Name = "Bug" },
                new TicketType { Id = 200, Name = "Task" },
                new TicketType { Id = 300, Name = "Documentation" });

            context.TicketStatuses.AddOrUpdate(
                t => t.Name,
                    new TicketStatus { Id = 100, Name = "New / UnAssigned" },
                    new TicketStatus { Id = 200, Name = "New / Assigned" },
                    new TicketStatus { Id = 300, Name = "In Progress" },
                    new TicketStatus { Id = 400, Name = "Closed" }
                    //new TicketStatus { Id = 500, Name = "Archived" },
                    //new TicketStatus { Id = 600, Name = "Waiting for Documentation" }
            );

            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Id = 100, Name = "1" },
                    new TicketPriority { Id = 200, Name = "2" },
                    new TicketPriority { Id = 300, Name = "3" },
                    new TicketPriority { Id = 400, Name = "4" },
                    new TicketPriority { Id = 500, Name = "5" }
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
                    DisplayName = "kerrydp8",
                    AvatarUrl = WebConfigurationManager.AppSettings["KerryAvatar"]
                }, "Wiiugamer12!"); ;
            }

            if (!context.Users.Any(u => u.Email == "DemoAdmin@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@Mailinator.com",
                    Email = "DemoAdmin@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin",
                    DisplayName = "DemoAdmin",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"]
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            if (!context.Users.Any(u => u.Email == "DustinHoffman@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DustinHoffman@Mailinator.com",
                    Email = "DustinHoffman@Mailinator.com",
                    FirstName = "Dustin",
                    LastName = "Hoffman",
                    DisplayName = "DHoff",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"]
                }, "Wiiugamer12!");
            }

            if (!context.Users.Any(u => u.Email == "DemoPM@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoPM@Mailinator.com",
                    Email = "DemoPM@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "PM",
                    DisplayName = "DemoPM",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"]
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }


            if (!context.Users.Any(u => u.Email == "LeonardoDiCaprio@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "LeonardoDiCaprio@Mailinator.com",
                    Email = "LeonardoDiCaprio@Mailinator.com",
                    FirstName = "Leonardo",
                    LastName = "DiCaprio",
                    DisplayName = "LDicap",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"]
                }, "Wiiugamer12!");
            }

            if (!context.Users.Any(u => u.Email == "DemoDeveloper@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDeveloper@Mailinator.com",
                    Email = "DemoDeveloper@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer",
                    DisplayName = "DemoDeveloper",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"]
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }


            if (!context.Users.Any(u => u.Email == "JessicaChastain@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "JessicaChastain@Mailinator.com",
                    Email = "JessicaChastain@Mailinator.com",
                    FirstName = "Jessica",
                    LastName = "Chastain",
                    DisplayName = "JChastain",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"]
                }, "Wiiugamer12!");
            }

            if (!context.Users.Any(u => u.Email == "DemoSubmitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSubmitter@Mailinator.com",
                    Email = "DemoSubmitter@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter",
                    DisplayName = "DemoSubmitter",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"]
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            var adminId = userManager.FindByEmail("kerrydp8@outlook.com").Id;
            userManager.AddToRole(adminId, "Administrator");

            adminId = userManager.FindByEmail("DemoAdmin@Mailinator.com").Id;
            userManager.AddToRole(adminId, "Administrator");

            var pmId = userManager.FindByEmail("DustinHoffman@Mailinator.com").Id;
            userManager.AddToRole(pmId, "Project Manager");

            pmId = userManager.FindByEmail("DemoPM@Mailinator.com").Id;
            userManager.AddToRole(pmId, "Project Manager");

            var devId = userManager.FindByEmail("LeonardoDiCaprio@Mailinator.com").Id;
            userManager.AddToRole(devId, "Developer");

            devId = userManager.FindByEmail("DemoDeveloper@Mailinator.com").Id;
            userManager.AddToRole(devId, "Developer");

            var subId = userManager.FindByEmail("JessicaChastain@Mailinator.com").Id;
            userManager.AddToRole(subId, "Submitter");

            subId = userManager.FindByEmail("DemoSubmitter@Mailinator.com").Id;
            userManager.AddToRole(subId, "Submitter");
        }
    }

}