using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Extensions;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TeamFinderDTO;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        protected readonly IApiClient _apiClient;

        public IndexModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IEnumerable<IGrouping<string, SessionResponse>> Sessions { get; set; }

        public GameDTO Game { get; set; }

        public IEnumerable<(int Offset, DateTime? Date)> DayOffsets { get; set; }

        public List<PlayerDTO> Owner { get; set; }

        public int CurrentDayOffset { get; set; }

        public bool IsAdmin { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public List<int> UserSessions { get; set; } = new List<int>();


        public async Task OnGet(int day = 0)
        {
            IsAdmin = User.IsAdmin();

            CurrentDayOffset = day;

            if (User.Identity.IsAuthenticated)
            {
                var userSessions = await _apiClient.GetSessionsByPlayerAsync(User.Identity.GetPlayerId());
                UserSessions = userSessions.Select(us => us.Id).ToList();
            }


            var sessions = await GetSessionsAsync();

            var startDate = DateTime.Today.Date;

            var offset = 0;
            DayOffsets = sessions.Select(s => s.StartTime?.Date)
                                 .Distinct()
                                 .Where(s => s.Value >= DateTime.Today)
                                 .OrderBy(d => d)
                                 .Select(day => (offset++, day?.Date));

            var filterDate = startDate.AddDays(CurrentDayOffset);

            Sessions = sessions.Where(s => s.StartTime?.Date == filterDate)
                               .GroupBy(s => s.Game.Name)
                               .OrderBy(g => g.Key);

            Owner = sessions.Select(s => s?.Players).FirstOrDefault();

            var gameId = sessions.Select(s => s.Game.Id);
            Game = await _apiClient.GetGameAsync(gameId.FirstOrDefault());
        }

        protected virtual Task<List<SessionResponse>> GetSessionsAsync()
        {
            return _apiClient.GetSessionsAsync();
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
