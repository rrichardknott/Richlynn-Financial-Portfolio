using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public virtual Household Household { get; set; }

        public string RecipientId { get; set; }
        public virtual ApplicationUser Recipient { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
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
        public bool IsRead { get; set; }
        public Notification()
        {
            Created = DateTime.Now;
        }

    }
}