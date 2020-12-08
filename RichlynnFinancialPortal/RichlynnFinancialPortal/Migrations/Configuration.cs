namespace RichlynnFinancialPortal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RichlynnFinancialPortal.Enums;
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


            #region Seed Household

            Household newHouse = null;
            if (!context.Households.Any())
            {
                newHouse = new Household
                {
                    Created = DateTime.Now,
                    Greeting = "Hello from Seeded Household",
                    IsDeleted = false,
                    HouseholdName = "Seeded Household"
                };

                context.Households.Add(newHouse);
            }
            context.SaveChanges();


           
            Household newHouse1 = new Household
            {
                Created = DateTime.Now,
                Greeting = "Hello from The Avalos Household",
                IsDeleted = false,
                HouseholdName = "The Avalos'"
            };

            context.Households.Add(newHouse1);
            context.SaveChanges();

            
            
            Household newHouse2 = new Household
            {
                Created = DateTime.Now,
                Greeting = "Hello from The Jones' Household",
                IsDeleted = false,
                HouseholdName = "The Jones'"
            };

            context.Households.Add(newHouse2);

            context.SaveChanges();
            #endregion
            

            if (!context.Users.Any(u => u.Email == headEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = headEmail,
                    UserName = headEmail,
                    FirstName = "Jose",
                    LastName = "Avalos",
                }, headPassword);

                var userId = userManager.FindByEmail(headEmail).Id;
                userManager.AddToRole(userId, "Head");
                var user = userManager.FindByEmail(headEmail);
                user.HouseholdId = context.Households.Where(h => h.HouseholdName == "The Avalos'").FirstOrDefault().Id;

            }

            if (!context.Users.Any(u => u.Email == memberEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = memberEmail,
                    UserName = memberEmail,
                    FirstName = "Ruth",
                    LastName = "Jones",
                }, memberPassword);
                
                var userId = userManager.FindByEmail(memberEmail).Id;                
                userManager.AddToRole(userId, "Member");
                var user = userManager.FindByEmail(memberEmail);
                user.HouseholdId = context.Households.Where(h => h.HouseholdName == "The Jones'").FirstOrDefault().Id;                


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

           

            #region Seed Bank Account
            var ownerId = context.Users.FirstOrDefault(u => u.Email == headEmail).Id;
            if (!context.BankAccounts.Any())
            {
                context.BankAccounts.Add(new BankAccount
                {
                    AccountName = "Wells Fargo Checking",
                    AccountType = AccountType.Checking,
                    Created = DateTime.Now,
                    CurrentBalance = 5000,
                    HouseholdId = newHouse.Id,
                    IsDeleted = false,
                    OwnerId = ownerId,
                    StartingBalance = 5000,
                    WarningBalance = 500
                });

                context.SaveChanges();
            }
            #endregion

            #region Seed Budget
            Budget budget = null;
            if (!context.Budgets.Any())
            {               

                budget = new Budget(true)
                {
                    BudgetName = "Utilities",
                    Created = DateTime.Now,
                    HouseholdId = newHouse.Id,
                    OwnerId = ownerId,
                    CurrentAmount = 0
                };

                context.Budgets.Add(budget);
                context.SaveChanges();

                Budget budget1 = null;
                budget1 = new Budget(true)
                {
                    BudgetName = "Groceries",
                    Created = DateTime.Now,
                    HouseholdId = newHouse1.Id,
                    OwnerId = ownerId,
                    CurrentAmount = 0
                };
            }
            #endregion

            #region Seed Item
            if (!context.BudgetItems.Any())
            {
                context.BudgetItems.Add(new BudgetItem()
                {
                    CurrentAmount = 0,
                    TargetAmount = 250,
                    BudgetId = budget.Id,
                    Created = DateTime.Now,
                    ItemName = "Gas Bill",
                    IsDeleted = false
                });
                context.SaveChanges();

                BudgetItem budgetItem1 = new BudgetItem
                {
                    CurrentAmount = 0,
                    TargetAmount = 250,
                    BudgetId = budget.Id,
                    Created = DateTime.Now,
                    ItemName = "Electricity Bill",
                    IsDeleted = false
                };
                context.SaveChanges();
                #endregion
            }
        }

    }

} 