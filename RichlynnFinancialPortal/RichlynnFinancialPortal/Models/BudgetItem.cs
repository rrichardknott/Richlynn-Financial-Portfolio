﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }
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


        [Display(Name = "Name")]
        public string ItemName { get; set; }

        [Display(Name = "Target Amount")]
        public decimal TargetAmount { get; set; }

        [Display(Name = "Current Amount")]
        public decimal CurrentAmount { get; set; }

        [Display(Name = "Delete Budget Item?")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public BudgetItem()
        {
            Transactions = new HashSet<Transaction>();
            Created = DateTime.Now;
        }
    }
}