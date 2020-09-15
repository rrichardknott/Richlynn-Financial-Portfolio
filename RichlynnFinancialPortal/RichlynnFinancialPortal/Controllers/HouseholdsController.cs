using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RichlynnFinancialPortal.Models;
using RichlynnFinancialPortal.Helpers;
using RichlynnFinancialPortal.Extensions;
using System.Threading.Tasks;
using RichlynnFinancialPortal.ViewModels;

namespace RichlynnFinancialPortal
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        [Authorize(Roles ="New User")]
        public ActionResult Create()
        {
            var userModel = new Household();//May have to change the model
            return View(userModel);
        }

        // POST: Households/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "New User")]
        public async Task<ActionResult> Create([Bind(Include = "Id,HouseholdName,Greeting")] Household household)
        {
            if (ModelState.IsValid) 
            {
                household.Created = DateTime.Now;
                db.Households.Add(household);
                db.SaveChanges();

                var user = db.Users.Find(User.Identity.GetUserId());
                user.HouseholdId = household.Id;

                foreach (var account in user.BankAccounts)
                {
                    account.HouseholdId = household.Id;
                }

                userRoleHelper.UpdateUserRole(user.Id, "Head");
                db.SaveChanges();

                await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);


                return RedirectToAction("ConfigureHouse");
            }

            return View(household);
        }

        [HttpGet]
        [Authorize(Roles ="Head")]
        public ActionResult ConfigureHouse()
        {
            var model = new ConfigureHouseViewModel();
            model.HouseholdId = User.Identity.GetHouseholdId();
            if (model.HouseholdId == 0)
            {
                return RedirectToAction("Create");
            }
            return View(model);            
        }

        [HttpPost]        
        [ValidateAntiForgeryToken]
        public ActionResult ConfigureHouse(ConfigureHouseViewModel model)
        {

            var bankAccount = new BankAccount(model.StartingBalance, model.BankAccount.WarningBalance, model.BankAccount.AccountName);
            bankAccount.AccountType = model.BankAccount.AccountType;
            bankAccount.HouseholdId = (int)model.HouseholdId;
            db.BankAccounts.Add(bankAccount);

            var budget = new Budget();
            budget.HouseholdId = (int)model.HouseholdId;
            budget.BudgetName = model.Budget.BudgetName;
            db.Budgets.Add(budget);

            db.SaveChanges();

            var budgetItem = new BudgetItem();
            budgetItem.BudgetId = budget.Id;
            budgetItem.TargetAmount = model.BudgetItem.TargetAmount;
            budgetItem.ItemName = model.BudgetItem.ItemName;
            db.BudgetItems.Add(budgetItem);

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdName,Greeting,Created,IsDeleted")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }


        // Leave a household: User leaving the house has his HHID set to null
        // If the user is in head of household role, someone must take over as head of household
        // If user is in member role then they can just leave
        // Anyone leaving a household needs their role reset to New User
        // If user is the last person of the household then the household can be deleted (soft delete)
        // async Task == void return type

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> LeaveAsync()
        {            

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var role = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (role)
            {
                case "Head":
                    var memberCount = db.Users.Where(u => u.HouseholdId == user.HouseholdId).Count() -1;
                    if (memberCount >= 1)
                    {
                        TempData["Mesage"] = $"You are unable to leave the household as there are still {memberCount} members left in the household. You must designate someone else as the head of household.";
                        return RedirectToAction("ExitDenied");
                    }                    

                    // soft delete - record stays in the DB
                    user.Household.IsDeleted = true;

                    //This is a hard delete, the record is removed from the DB and anything with the Household's FK will be cascade deleted
                    //var household = db.Households.Find(user.HouseholdId);
                    //db.Households.Remove(household);
                    user.HouseholdId = null;


                    //Remove the household Id from all accounts associated with this user
                    foreach (var account in user.BankAccounts)
                    {
                        account.HouseholdId = null;
                    }

                    db.SaveChanges();

                    userRoleHelper.UpdateUserRole(userId, "New User");
                    await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
                    return RedirectToAction("Home", "Index");

                case "Member":
                    user.HouseholdId = null;
                    //Remove the household Id from all accounts associated with this user
                    foreach (var account in user.BankAccounts)
                    {
                        account.HouseholdId = null;
                    }
                    db.SaveChanges();

                    userRoleHelper.UpdateUserRole(userId, "New User");
                    await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
                    return RedirectToAction("Home", "Index");
                    
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Head")]
        public ActionResult ExitDenied()
        {
            return View();
        }


        [Authorize(Roles = "Head")]
        public ActionResult ChangeHead()
        {

            var myHouseId = User.Identity.GetHouseholdId();
            if (myHouseId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            var members = db.Users.Where(u => u.HouseholdId == myHouseId).ToList();
            ViewBag.NewHoH = new SelectList(members, "Id", "FullName");

            return View();
        }

        [HttpPost, ActionName("ChangeHead")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeHeadAsync(string newHoH, bool leave)
        {
            if (string.IsNullOrEmpty(newHoH) || newHoH == User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Home");
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            userRoleHelper.UpdateUserRole(newHoH, "Head");
            if (leave == true)
            {
                user.HouseholdId = null;

                //Remove the HouseholdId from all BankAccounts associated with this user
                foreach (var account in user.BankAccounts)
                {
                    account.HouseholdId = null;
                }
                db.SaveChanges();
                userRoleHelper.UpdateUserRole(user.Id, "New User");
                await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
            }

            else
            {
                userRoleHelper.UpdateUserRole(user.Id, "Member");
                await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
            }

            return RedirectToAction("Index", "Home");
        }






        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
