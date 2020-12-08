using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RichlynnFinancialPortal.Models;
using RichlynnFinancialPortal.Enums;
using Microsoft.AspNet.Identity;
using RichlynnFinancialPortal.Extensions;
using RichlynnFinancialPortal.ViewModels;

namespace RichlynnFinancialPortal
{
    [Authorize(Roles = "New User, Member, Head")]
    public class BankAccountsController : Controller
    {
        

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BankAccounts
       
        public ActionResult Index()
        {
            //var bankAccounts = db.BankAccounts.Include(b => b.Household).Include(b => b.Owner);
            var data = new BankAccountIndexViewModel();
            var userId = User.Identity.GetUserId();
            data.MyBankAccounts = db.BankAccounts.Where(b => b.OwnerId == userId).ToList();
            var myHouseholdId = User.Identity.GetHouseholdId();
            if (myHouseholdId != null)
            {
                data.HouseholdAccounts = db.Users.Where(u => u.HouseholdId == myHouseholdId && u.Id != userId).SelectMany(u => u.BankAccounts).ToList();
            }
           
            return View(data);
        }

        // GET: BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        [Authorize(Roles ="Member, Head")]
        public ActionResult Create()
        {
            var userModel = new BankAccount();
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "HouseholdName");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName");
            return View(userModel);
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BankAccount account, decimal startingBalance)
        {
            if (ModelState.IsValid)
            {
                account.Created = DateTime.Now;
                account.OwnerId = User.Identity.GetUserId();
                account.HouseholdId = User.Identity.GetHouseholdId();
                account.StartingBalance = startingBalance;
                account.CurrentBalance = account.StartingBalance;
                db.BankAccounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "HouseholdName", bankAccount.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", bankAccount.OwnerId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountName,CurrentBalance,WarningBalance,IsDeleted,AccountType")] BankAccount bankAccount, decimal startingBalance)
        {
            if (ModelState.IsValid)
            {
                bankAccount.Created = DateTime.Now;
                bankAccount.OwnerId = User.Identity.GetUserId();
                bankAccount.HouseholdId = User.Identity.GetHouseholdId();
                bankAccount.StartingBalance = startingBalance;                
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "HouseholdName", bankAccount.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", bankAccount.OwnerId);
            return View(bankAccount);
        }


        public PartialViewResult _BankAccountModal()
        {
            var userModel = new BankAccount();

            return PartialView(userModel);
        }






        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccounts.Find(id);
            db.BankAccounts.Remove(bankAccount);
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
