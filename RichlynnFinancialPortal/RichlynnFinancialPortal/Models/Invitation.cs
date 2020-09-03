﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int TTL { get; internal set; }// Time To Live - number of days invitation is valid

        //if(DateTime.Now > Created.AddDays(TTL)){IsValid = false};

        //=======================================================================//

        [Display(Name = "Recipient Email")]
        public string RecipientEmail { get; set; }

        public Guid Code { get; set; }

        public Invitation()
        {
            Created = DateTime.Now;
            IsValid = true;
            TTL = 3;
        }
    }
}