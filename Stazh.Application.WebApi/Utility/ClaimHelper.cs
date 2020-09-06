using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stazh.Application.WebApi.Utility
{
    public class ClaimHelper
    {
        public static string GetUserIdFromClaim(IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(o => o.Type == @"http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        }
    }
}
