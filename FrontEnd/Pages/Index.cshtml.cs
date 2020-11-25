using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public List<(int Offset, DateTime? Date)> DayOffsets { get; set; }

        public List<PlayerDTO> Owner { get; set; }

        public int CurrentDayOffset { get; set; }

        public int CurrentNumberOfPlayers { get; set; }

        public bool IsAdmin { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public List<int> UserSessions { get; set; } = new List<int>();

        public async Task OnGet(string date = "")
        {
            IsAdmin = User.IsAdmin();
            DateTime dateTime = new DateTime();

            dateTime = ConvertToDateTime(dateTime, date);

            CurrentDayOffset = (dateTime.Date - DateTime.Today.Date).Days;

            if (User.Identity.IsAuthenticated)
            {
                var userSessions = await _apiClient.GetSessionsByPlayerAsync(User.Identity.Name);
                UserSessions = userSessions.Select(us => us.Id).ToList();
            }

            var sessions = await GetSessionsAsync();

            DayOffsets = CreateSevenDaysOffset();
            
            var filterDate = DateTime.Today.AddDays(CurrentDayOffset);

            Sessions = sessions.Where(s => s.StartTime?.Date == filterDate)                         
                               .GroupBy(s => s.Game.Name)
                               .OrderBy(g => g.Key);
        }

        protected virtual Task<List<SessionResponse>> GetSessionsAsync()
        {
            return _apiClient.GetSessionsAsync();
        }

        public async Task<IActionResult> OnPostAsync(int sessionId)
        {
            await _apiClient.AddSessionToPlayerAsync(User.Identity.Name, sessionId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int sessionId)
        {
            await _apiClient.RemoveSessionFromPlayerAsync(User.Identity.Name, sessionId);

            return RedirectToPage();
        }

        private DateTime ConvertToDateTime(DateTime dateTime, string date)
        {

            if (String.IsNullOrEmpty(date))
            {
                dateTime = DateTime.Today.Date;

            }
            else
            {
                dateTime = Convert.ToDateTime(date);
            }

            return dateTime;
        }

        private List<(int, DateTime?)>  CreateSevenDaysOffset()
        {
            var DayOffsets = new List<(int, DateTime?)>();

            for (var i = 0; i < 7; i++)
            {
                DayOffsets.Add((i, DateTime.Today.AddDays(i)));
            }

            return DayOffsets;
        }
    }
}
