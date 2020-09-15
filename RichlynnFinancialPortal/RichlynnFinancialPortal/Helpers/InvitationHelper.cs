using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RichlynnFinancialPortal.Models;


namespace RichlynnFinancialPortal.Helpers
{
    public static class InvitationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void MarkAsInvalid(int id)
        {
            var invitation = db.Invitations.Find(id);
            invitation.IsValid = false;

            db.SaveChanges();
        }
    }
}