using Microsoft.AspNet.Identity;
using RichlynnFinancialPortal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Display(Name="Bank Account")]
        public int AccountId { get; set; }

        public virtual BankAccount Account { get; set; }

        public int? BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        public TransactionType TransactionType {get; set;}

        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public DateTime Created { get; set; }

        public decimal Amount { get; set; }
        public string Memo { get; set; } // description of transaction

        [Display(Name = "Delete Transaction?")]
        public bool IsDeleted { get; set; }

        public Transaction()
        {
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
        }

    }
}