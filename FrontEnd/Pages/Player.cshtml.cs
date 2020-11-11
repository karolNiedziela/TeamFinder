using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamFinderDTO;

namespace FrontEnd
{
    public class PlayerModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public PlayerModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public PlayerResponse Player { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Player = await _apiClient.GetPlayerAsync(id);

            if (Player == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
