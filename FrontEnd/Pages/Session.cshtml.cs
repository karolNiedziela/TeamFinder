using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamFinderDTO;

namespace FrontEnd.Pages
{
    public class SessionModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public SessionModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public SessionResponse Session { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }

        public PlayerDTO Owner { get; set; }

        public bool IsInProfile { get; set; }
        
        public bool IsOwner { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Session = await _apiClient.GetSessionAsync(id);

            if (Session.Players.Count == 0)
            {
                await _apiClient.DeleteSessionAsync(Session.Id);
                return RedirectToPage("/Index");
            }

            if (Session == null)
            {
                return RedirectToPage("/Index");
            }

            if (User.Identity.IsAuthenticated)
            {
                var sessions = await _apiClient.GetSessionsByPlayerAsync(User.Identity.Name);

                IsInProfile = sessions.Any(s => s.Id == id);
            }

            Owner = Session.Owner;

            IsOwner = User.IsOwner(id);

            DayOfWeek = Session.StartTime?.DayOfWeek;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int sessionId)
        {
            await _apiClient.AddSessionToPlayerAsync(User.Identity.Name, sessionId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int sessionId)
        {
            if (User.IsOwner(sessionId))
            {
                await _apiClient.DeleteSessionAsync(sessionId);
                return RedirectToPage("/Index");
            }

            await _apiClient.RemoveSessionFromPlayerAsync(User.Identity.Name, sessionId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostKickAsync(string username, int sessionId)
        {
            await _apiClient.RemoveSessionFromPlayerAsync(username, sessionId);

            return RedirectToPage();
        }

    }
}
