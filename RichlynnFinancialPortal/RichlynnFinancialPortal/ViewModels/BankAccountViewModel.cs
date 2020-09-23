using RichlynnFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.ViewModels
{
    public class BankAccountIndexViewModel
    {

        public List<BankAccount> MyBankAccounts { get; set; } = new List<BankAccount>();
        public List<BankAccount> HouseholdAccounts { get; set; } = new List<BankAccount>();
    }
}