﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace RichlynnFinancialPortal.Extensions
{
    public static class IdentityExtensions
    {
        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var householdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            if (householdClaim != null)
            {
                var result = householdClaim.Value != "" ? int.Parse(householdClaim.Value) : 0;
                return result;
            }
            else
            {
                return null;
            }
        }

        public static string GetFullName(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var fullNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "FullName");
            return fullNameClaim != null ? fullNameClaim.Value : null;//this says:  if (fullNameClaim != null){return fullNameClaim.Value} else {return null}
        }

        public static string GetAvatarPath(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var avatarClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "AvatarPath");
            return avatarClaim != null ? avatarClaim.Value : null;//this says:  if (avatarClaim != null){return avatarClaim.Value} else {return null}
        }
       
    }
}