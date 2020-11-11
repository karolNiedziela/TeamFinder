using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamFinderDTO;

namespace FrontEnd.Services
{
    public interface IApiClient
    {
        Task<List<SessionResponse>> GetSessionsAsync();
        Task<SessionResponse> GetSessionAsync(int id);
        Task<List<PlayerResponse>> GetPlayersAsync();
        Task<PlayerResponse> GetPlayerAsync(int id);
        Task<List<PlayerResponse>> GetPlayerGamesAsync(int id);
        Task<List<SessionResponse>> GetPlayerSessionsAsync(int id);
        Task<List<GameDTO>> GetGamesAsync();
        Task<GameDTO> GetGameAsync(string name);
        Task<SessionResponse> PostSession(SessionDTO session);
        Task<List<SearchResult>> SearchAsync(string query);
        Task PutSessionAsync(SessionDTO session);
        Task DeleteSessionAsync(int id);
    }
}
