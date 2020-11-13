using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamFinderDTO;

namespace FrontEnd.Areas.Identity.Pages.Account.Manage
{
    public class PlayerModel : PageModel
    {
        private readonly IApiClient _apiClient;
        private readonly UserManager<User> _userManager;

        public PlayerModel(IApiClient apiClient, UserManager<User> userManager)
        {
            _apiClient = apiClient;
            _userManager = userManager;
        }

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public PlayerDTO Player { get; set; }        

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var player = await _apiClient.GetPlayerAsync(user.UserName);


            Player = new PlayerDTO
            {
                Id = player.Id,
                UserName = player.UserName,
                Age = player.Age,
                Info = player.Info
            };


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Session updated successfully";

            await _apiClient.PutPlayerAsync(Player);

            return RedirectToPage();
        }
    }
}
