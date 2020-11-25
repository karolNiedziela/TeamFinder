using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamFinderDTO;

namespace FrontEnd.Pages
{
    public class GamesModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public bool IsInProfile { get; set; }

        public GamesModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<GameDTO> Games { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Games = await _apiClient.GetGamesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int gameId)
        {
            await _apiClient.AddGameToPlayerAsync(User.Identity.Name, gameId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int gameId)
        {
            await _apiClient.RemoveGameFromPlayerAsync(User.Identity.Name, gameId);

            return RedirectToPage();
        }

        public async Task<bool> CheckIfInProfile(int gameId)
        {
            var playerGames = await _apiClient.GetPlayerGamesAsync(User.Identity.Name);

            if (playerGames.Any(p => p.Games.Any(g => g.Id == gameId)))
                return false;

            return true;
        }
    }
}
