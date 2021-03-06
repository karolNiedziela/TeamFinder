﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FrontEnd.Middleware
{
    public class RequireLoginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LinkGenerator _linkGenerator;

        public RequireLoginMiddleware(RequestDelegate next, LinkGenerator linkGenerator)
        {
            _next = next;
            _linkGenerator = linkGenerator;
        }

        public Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (context.User.Identity.IsAuthenticated && endpoint?.Metadata.GetMetadata<SkipWelcomeAttribute>() == null)
            {
                var isPlayer = context.User.IsPlayer();

                if (!isPlayer)
                {
                    var url = _linkGenerator.GetUriByPage(context, page: "/Welcome");
                    context.Response.Redirect(url);

                    return Task.CompletedTask;
                }
            }

            return _next(context);
        }
    }
}
