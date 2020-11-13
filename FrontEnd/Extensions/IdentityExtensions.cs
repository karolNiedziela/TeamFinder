using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FrontEnd.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetPlayerId(this IIdentity identity)
        {
            IEnumerable<Claim> claims = ((ClaimsIdentity)identity).Claims;
            var playerId = claims.Where(c => c.Type == "PlayerId").SingleOrDefault();

            return Int32.Parse(playerId.Value);
        }
    }
}
