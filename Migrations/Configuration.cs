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

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string DisplayName { get; private set; }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

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

            if (!context.Users.Any(u => u.Email == "DustinHoffman@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DustinHoffman@Mailinator.com",
                    Email = "DustinHoffman@Mailinator.com",
                    FirstName = "Dustin",
                    LastName = "Hoffman",
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