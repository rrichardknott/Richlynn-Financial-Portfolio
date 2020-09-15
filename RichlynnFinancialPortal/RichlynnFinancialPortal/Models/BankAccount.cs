using Microsoft.AspNet.Identity;
using RichlynnFinancialPortal.Enums;
using RichlynnFinancialPortal.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.Models
{
    public class BankAccount
    {

        public int Id { get; set; }

        public int? HouseholdId { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual Household Household { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        [Display(Name = "Bank Account Name")]
        public string AccountName { get; set; }

        
        public DateTime Created { get; set; }
        [NotMapped]
        [Display(Name = "Created")]
        public string CreatedString
        {
            get
            {
                string dateString = Created.ToString("MMM dd, yyy");
                return dateString;
            }
        }

        [Display(Name = "Starting Balance")]
        public decimal StartingBalance { get; internal set; }

        [Display(Name = "Current Balance")]
        public decimal CurrentBalance { get; set; }

        [Display(Name = "Warning Balance")]
        public decimal WarningBalance { get; set; }

        [Display(Name = "Delete Account?")]
        public bool IsDeleted { get; set; } // considered a soft delete because it can be toggled from true/false

        public virtual ICollection<Transaction> Transactions { get; set; }// ICollection need hash set

        public AccountType AccountType { get; set; }

        public BankAccount(decimal startingBalance, decimal warningBalance, string accountName)
        {
            Transactions = new HashSet<Transaction>();
            StartingBalance = startingBalance;
            CurrentBalance = startingBalance;// CurrentBalance will equal startingBalance upon creation
            WarningBalance = warningBalance;
            //AccountType = AccountType;
            Created = DateTime.Now;
            AccountName = accountName;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
            HouseholdId = (int)HttpContext.Current.User.Identity.GetHouseholdId();
        }

        public BankAccount()
        {
            StartingBalance = -1;// set to negative to test against starting balance
        }

    }
}