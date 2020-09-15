using RichlynnFinancialPortal.Enums;
using RichlynnFinancialPortal.Models;
using System.Collections;
using System.Collections.Generic;

namespace RichlynnFinancialPortal.ViewModels
{
    public class BankAccountWizardViewModel

    {
        public decimal StartingBalance { get; set; }
        public decimal WarningBalance { get; set; }
        public decimal AccountName { get; set; }

        public AccountType AccountType { get; set; }

    }

}