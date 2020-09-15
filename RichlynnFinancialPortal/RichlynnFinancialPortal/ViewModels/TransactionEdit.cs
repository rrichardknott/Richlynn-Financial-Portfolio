using RichlynnFinancialPortal.Enums;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.ViewModels
{
    public class TransactionEdit
    {


        public int Id { get; set; }

        [Display(Name = "Bank Account")]
        public int AccountId { get; set; }
        public int? BudgetItemId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }

        [Display(Name = "Delete Transaction")]
        public bool IsDeleted { get; set; }


    }
}