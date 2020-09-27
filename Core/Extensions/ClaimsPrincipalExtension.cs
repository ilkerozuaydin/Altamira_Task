using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }

        public static Guid UserID(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindAll(ClaimTypes.NameIdentifier)?.Select(x => x.Value).SingleOrDefault();
            return new Guid(result);
        }

        public static string UserName(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindAll(ClaimTypes.Name)?.Select(x => x.Value).SingleOrDefault();
            return result;
        }

        public static string Email(this ClaimsPrincipal claimsPrincipal)
        {
            var result = claimsPrincipal?.FindAll(ClaimTypes.Email)?.Select(x => x.Value).SingleOrDefault();
            return result;
        }
    }
}