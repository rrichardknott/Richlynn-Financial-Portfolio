using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RichlynnFinancialPortal.Models;

namespace RichlynnFinancialPortal.ViewModels
{
    public class ConfigureHouseViewModel
    {
        public ConfigureHouseViewModel()
        {
            BankAccount = new BankAccount();
            Budget = new Budget();
            BudgetItem = new BudgetItem();
        }


        public int? HouseholdId { get; set; }

        //What makes a household?  One or more Bank Accounts, Budgets, and Budget Items
        public BankAccount BankAccount { get; set; }

        //public Decimal StartingBalance { get; set; }

        public decimal StartingBalance { get; set; }

        public Budget Budget { get; set; }

        public BudgetItem BudgetItem { get; set; }

        //public ICollection<BankAccount> BankAccounts { get; set; }       
        //public ICollection<BudgetItem> BudgetItems { get; set; }
        //public ICollection<Budget> Budgets { get; set; }
        //public ICollection<BankAccountWizardViewModel> BankAccounts { get; set; }
        //public ICollection<BudgetWizardViewModel> Budgets { get; set; }

    }
}