using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Data;
using FrontEnd.Middleware;
using FrontEnd.Pages.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    [SkipWelcome]
    public class WelcomeModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public WelcomeModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Player Player { get; set; }



        public IActionResult OnGet()
        {
            var isPlayer = User.IsPlayer();

            if (!User.Identity.IsAuthenticated || isPlayer)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var success = await _apiClient.AddPlayerAsync(Player);

            if (!success)
            {
                ModelState.AddModelError("", "There was an issue creating the player for this user.");
                return Page();
            }

            User.MakePlayer();
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, User);

            return RedirectToPage("/Index");
        }
    }
}
