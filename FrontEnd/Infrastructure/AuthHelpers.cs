using FrontEnd.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Infrastructure
{
    public static class AuthConstants
    {
        public static readonly string IsAdmin = nameof(IsAdmin);
        public static readonly string IsPlayer = nameof(IsPlayer);
        public static readonly string TrueValue = "true";
    }
}

namespace System.Security.Claims
{
    public static class AuthenticationHelpers
    {
        public static bool IsAdmin(this ClaimsPrincipal principal) =>
            principal.HasClaim(AuthConstants.IsAdmin, AuthConstants.TrueValue);

        public static void MakeAdmin(this ClaimsPrincipal principal) =>
            principal.Identities.First().MakeAdmin();

        public static void MakeAdmin(this ClaimsIdentity identity) =>
            identity.AddClaim(new Claim(AuthConstants.IsAdmin, AuthConstants.TrueValue));

        public static bool IsPlayer(this ClaimsPrincipal principal) =>
            principal.HasClaim(AuthConstants.IsPlayer, AuthConstants.TrueValue);

        public static void MakePlayer(this ClaimsPrincipal principal) =>
            principal.Identities.First().MakePlayer();

        public static void MakePlayer(this ClaimsIdentity identity) =>
            identity.AddClaim(new Claim(AuthConstants.IsPlayer, AuthConstants.TrueValue));
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizationHelpers
    {
        public static AuthorizationPolicyBuilder RequireIsAdminClaim(this AuthorizationPolicyBuilder builder) =>
            builder.RequireClaim(AuthConstants.IsAdmin, AuthConstants.TrueValue);

        public static AuthorizationPolicyBuilder RequireIsPlayerClaim(this AuthorizationPolicyBuilder builder) =>
            builder.RequireClaim(AuthConstants.IsPlayer, AuthConstants.TrueValue);
    }
}
