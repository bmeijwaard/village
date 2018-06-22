using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Village.Data.Identity
{
    public static class IdenityExtensions
    {
        public static string UserName(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        public static string Firstname(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
        public static string Lastname(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;
        public static Guid Id(this IIdentity identity) => new Guid(((ClaimsIdentity)identity).FindFirst(ClaimTypes.Sid)?.Value ?? Guid.Empty.ToString());
        public static bool IsAdmin(this IIdentity identity) => ((ClaimsIdentity)identity).FindFirst(ClaimsStore.ADMINISTRATOR)?.Value == "True";
    }
}
