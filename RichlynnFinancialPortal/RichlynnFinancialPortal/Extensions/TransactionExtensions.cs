using RichlynnFinancialPortal.Models;
using RichlynnFinancialPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.Extensions
{
   
    public static class TransactionExtensions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        //If transactionType.deposit -> increase current amount of bankaccount
        //If trasactionType.withdrawal -> decrease current amount of bankaccount, increase the current amount of budget and budgetItem
        //If transactionType.transfer -> decrease current amount of bankaccount1 and increase current amount of bankaccount2
        public static void UpdateBalances(this Transaction transaction)
        {
            UpdateBankBalance(transaction);

            
            if (transaction.TransactionType == Enums.TransactionType.Withdrawal)
            {                
                UpdateBudgetAmount(transaction);
                UpdateBudgetItemAmount(transaction);                
            }
           
        }

        public static void EditTransaction(this Transaction newTransaction, Transaction oldTransaction)
        {
            //What happens when I Edit a transaction? - could I use a momento object:  .AsNoTracking()
            //What happens when I Delete or Void a transaction? - Part of these methods involve tracking the old amount and the new amount

            if (oldTransaction.TransactionType == Enums.TransactionType.Deposit)
            {
                ReverseUpdateBankBalance(oldTransaction);
                UpdateBankBalance(newTransaction);
            }
            if (oldTransaction.TransactionType == Enums.TransactionType.Withdrawal)
            {
                ReverseUpdateBankBalance(oldTransaction);
                UpdateBankBalance(newTransaction);
                ReverseUpdateBudgetAmount(oldTransaction);
                UpdateBudgetAmount(newTransaction);
                ReverseUpdateBudgetItemAmount(oldTransaction);
                UpdateBudgetItemAmount(newTransaction);
            }
        }

        private static void UpdateBankBalance(Transaction transaction)
        {
            var bankAccount = db.BankAccounts.Find(transaction.AccountId);

            if (transaction.TransactionType == Enums.TransactionType.Deposit)
            {
                bankAccount.CurrentBalance += transaction.Amount;
            }
            else if (transaction.TransactionType == Enums.TransactionType.Withdrawal)
            {
                bankAccount.CurrentBalance -= transaction.Amount;
            }
            db.SaveChanges();
        }       


        private static void UpdateBudgetAmount(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            var budget = db.Budgets.Find(budgetItem.BudgetId);
            budget.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }

        private static void UpdateBudgetItemAmount(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            budgetItem.CurrentAmount += transaction.Amount;
            db.SaveChanges();
        }

        private static void ReverseUpdateBankBalance(Transaction transaction)
        {
            var bankAccount = db.BankAccounts.Find(transaction.AccountId);
            switch (transaction.TransactionType)
            {
                case TransactionType.Deposit:
                    bankAccount.CurrentBalance -= transaction.Amount;
                    break;
                case TransactionType.Withdrawal:
                    bankAccount.CurrentBalance += transaction.Amount;
                    break;
                default:
                    return;
            }
            db.SaveChanges();
        }

        private static void ReverseUpdateBudgetAmount(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            var budget = db.Budgets.Find(budgetItem.BudgetId);
            budget.CurrentAmount -= transaction.Amount;
            db.SaveChanges();
        }

        private static void ReverseUpdateBudgetItemAmount(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            budgetItem.CurrentAmount -= transaction.Amount;
            db.SaveChanges();
        }

    }
}