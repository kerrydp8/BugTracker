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
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"],
                    DisplayName = "kerrydp8"
                }, "Wiiugamer12");
            }

            if (!context.Users.Any(u => u.Email == "DustinHoffman@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DustinHoffman@Mailinator.com",
                    Email = "DustinHoffman@Mailinator.com",
                    FirstName = "Dustin",
                    LastName = "Hoffman",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"],
                    DisplayName = "DHoff"
                }, "DustinHoffman1!");
            }


            if (!context.Users.Any(u => u.Email == "LeonardoDiCaprio@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "LeonardoDiCaprio@Mailinator.com",
                    Email = "LeonardoDiCaprio@Mailinator.com",
                    FirstName = "Leonardo",
                    LastName = "DiCaprio",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"],
                    DisplayName = "LDicap"
                }, "LeonardoDiCaprio2!");
            }


            if (!context.Users.Any(u => u.Email == "JessicaChastain@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "JessicaChastain@Mailinator.com",
                    Email = "JessicaChastain@Mailinator.com",
                    FirstName = "Jessica",
                    LastName = "Chastain",
                    AvatarUrl = WebConfigurationManager.AppSettings["DefaultAvatar"],
                    DisplayName = "JChastain"
                }, "JessicaChastain3!");
            }

            var userId = userManager.FindByEmail("kerrydp8@outlook.com").Id;
            userManager.AddToRole(userId, "Administrator");

            userId = userManager.FindByEmail("DustinHoffman@Mailinator.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            userId = userManager.FindByEmail("LeonardoDiCaprio@Mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            userId = userManager.FindByEmail("JessicaChastain@Mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");
        }
    }

}