using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Extensions;
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

        public bool IsInProfile { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Session = await _apiClient.GetSessionAsync(id);

            if (Session == null)
            {
                return RedirectToPage("/Index");
            }

            if (User.Identity.IsAuthenticated)
            {
                var sessions = await _apiClient.GetSessionsByPlayerAsync(User.Identity.GetPlayerId());

                IsInProfile = sessions.Any(s => s.Id == id);
            }

            var allSessions = await _apiClient.GetSessionsAsync();

            var startDate = allSessions.Min(s => s.StartTime?.Date);

            DayOfWeek = Session.StartTime?.DayOfWeek;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int sessionId)
        {
            await _apiClient.AddSessionToPlayerAsync(User.Identity.GetPlayerId(), sessionId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int sessionId)
        {
            await _apiClient.RemoveSessionFromPlayerAsync(User.Identity.GetPlayerId(), sessionId);

            return RedirectToPage();
        }
    }
}
