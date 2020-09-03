namespace RichlynnFinancialPortal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RichlynnFinancialPortal.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<RichlynnFinancialPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(RichlynnFinancialPortal.Models.ApplicationDbContext context)
        {
            
            #region Role Creation
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            //Head of Household role
            if (!context.Roles.Any(r => r.Name == "Head"))
            {
                roleManager.Create(new IdentityRole { Name = "Head" });
            }

            //Represents a user who is part of a household, but not in the Head of Household role
            //We will assign this role to any user that registers with an invitation code or who enters an invitation code to join a household
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }

            //Represents a new user who has not joined or created a household
            //We will assign this role to anyone who registers an account without using an invitation code
            //This role will also be assigned to any user that leaves a household
            if (!context.Roles.Any(r => r.Name == "New User"))
            {
                roleManager.Create(new IdentityRole { Name = "New User" });
            }
            #endregion

            #region User Creation
            var adminEmail = WebConfigurationManager.AppSettings["AdminEmail"];
            var adminPassword = WebConfigurationManager.AppSettings["AdminPassword"];
            var headEmail = WebConfigurationManager.AppSettings["HeadEmail"];
            var headPassword = WebConfigurationManager.AppSettings["HeadPassword"];
            var memberEmail = WebConfigurationManager.AppSettings["MemberEmail"];
            var memberPassword = WebConfigurationManager.AppSettings["MemberPassword"];
            var newUserEmail = WebConfigurationManager.AppSettings["NewUserEmail"];
            var newUserPassword = WebConfigurationManager.AppSettings["NewUserPassword"];

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            // seeding myself in all roles for quick testing purposes
            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "Richard",
                    LastName = "Knott",
                }, adminPassword);

                var userId = userManager.FindByEmail(adminEmail).Id;
                userManager.AddToRole(userId, "Admin");

            }

            if (!context.Users.Any(u => u.Email == headEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = headEmail,
                    UserName = headEmail,
                    FirstName = "Richard",
                    LastName = "Knott",
                }, headPassword);

                var userId = userManager.FindByEmail(headEmail).Id;
                userManager.AddToRole(userId, "Head");

            }

            if (!context.Users.Any(u => u.Email == memberEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = memberEmail,
                    UserName = memberEmail,
                    FirstName = "Richard",
                    LastName = "Knott",
                }, memberPassword);

                var userId = userManager.FindByEmail(memberEmail).Id;
                userManager.AddToRole(userId, "Member");

            }

            if (!context.Users.Any(u => u.Email == newUserEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = newUserEmail,
                    UserName = newUserEmail,
                    FirstName = "Richard",
                    LastName = "Knott",
                }, newUserPassword);

                var userId = userManager.FindByEmail(newUserEmail).Id;
                userManager.AddToRole(userId, "New User");

            }
            #endregion
        }

    }

} 