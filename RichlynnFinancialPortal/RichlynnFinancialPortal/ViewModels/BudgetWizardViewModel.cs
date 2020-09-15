using RichlynnFinancialPortal.Migrations;
using RichlynnFinancialPortal.Models;
using RichlynnFinancialPortal.ViewModels;
using System.Collections.Generic;

namespace RichlynnFinancialPortal.ViewModels
{
    public class BudgetWizardViewModel
    {
        public Budget Budgets { get; set; }
        public ICollection<BudgetItem> BudgetItems { get; set; }
    }
}