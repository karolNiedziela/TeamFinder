using FrontEnd.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FrontEnd.Areas
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        private readonly IApiClient _apiClient;
        private readonly IHttpContextAccessor _accessor;

        public ClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor, IApiClient apiClient,
            IHttpContextAccessor accessor) 
            : base(userManager, optionsAccessor)
        {
            _apiClient = apiClient;
            _accessor = accessor;
        }

        protected async override Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (user.IsAdmin)
            {
                identity.MakeAdmin();
            }

            var player = await _apiClient.GetPlayerAsync(user.UserName);
            if (player != null)
            {
                identity.MakePlayer();
            }

            var playerSessions = await _apiClient.GetSessionsByPlayerAsync(user.UserName);
            if (player != null)
            {
                var playerIsOwner = playerSessions.Where(ps => ps.Owner.Id == player.Id);

                foreach (var session in playerIsOwner)
                {
                    var sessionId = session.Id;
                    identity.MakeOwner(sessionId);
                }
            }

            return identity;
        }
    }
}
