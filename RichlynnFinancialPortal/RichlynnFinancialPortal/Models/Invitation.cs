using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RichlynnFinancialPortal.Models
{
    public class Invitation
    {

        public int Id { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public string Body { get; set; }

        public bool IsValid { get; set; }// start as true and change under certain circumstances

        //========================================================================//
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

        public int TTL { get; internal set; }// Internally set by the constructor below.  Time To Live - number of days invitation is valid

        //if(DateTime.Now > Created.AddDays(TTL)){IsValid = false};

        //=======================================================================//

        [Display(Name = "Recipient Email")]
        public string RecipientEmail { get; set; }

        public Guid Code { get; set; }

        public Invitation(int hhId)
        {
            Created = DateTime.Now;
            IsValid = true;
            TTL = 3;
            HouseholdId = hhId;
        }

        public Invitation()
        {
            Created = DateTime.Now;
            IsValid = true;
            TTL = 3;
            
        }
    }
}