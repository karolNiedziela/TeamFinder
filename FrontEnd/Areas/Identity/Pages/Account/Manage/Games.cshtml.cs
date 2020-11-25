using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamFinderDTO;

namespace FrontEnd.Areas.Identity.Pages.Account
{
    public class GamesModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public List<GameDTO> Games { get; set; }

        public GamesModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var playerGames = await _apiClient.GetPlayerGamesAsync(User.Identity.Name);

            Games = playerGames.SelectMany(pg => pg.Games).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int gameId)
        {
            await _apiClient.RemoveGameFromPlayerAsync(User.Identity.Name, gameId);

            return RedirectToPage();
        }
    }
}
