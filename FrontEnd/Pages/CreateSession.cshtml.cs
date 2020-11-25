using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeamFinderDTO;
using FrontEnd.Pages.Models;

namespace FrontEnd.Pages
{
    public class CreateSessionModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public CreateSessionModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Session Session { get; set; }

        [BindProperty]
        public int SelectedGameId { get; set; }

        public List<SelectListItem> GamesSelectList { get; set; } 

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task<IActionResult> OnGetAsync()
        {
            var isPlayer = User.IsPlayer();

            if (!User.Identity.IsAuthenticated && !isPlayer)
            {
                return RedirectToPage("/Index");
            }

            await PopulateGamesDropDownList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateGamesDropDownList();
                return Page();
            }

            Message = "Session created successfully";

            await _apiClient.PostSession(Session, User.Identity.Name);

            return RedirectToPage("/Index");
        }



        public async Task PopulateGamesDropDownList()
        {
            var games = await _apiClient.GetGamesAsync();

            GamesSelectList = games.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            })
            .ToList();
        }
    }
}
