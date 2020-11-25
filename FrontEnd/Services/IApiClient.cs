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
        Task<PlayerResponse> GetPlayerAsync(string username);
        Task<List<PlayerResponse>> GetPlayerGamesAsync(string username);
        Task<List<GameDTO>> GetGamesAsync();
        Task<GameDTO> GetGameAsync(int id);
        Task<SessionResponse> PostSession(SessionDTO session, string username);
        Task<List<SearchResult>> SearchAsync(string query);
        Task PutSessionAsync(SessionDTO session);
        Task DeleteSessionAsync(int id);
        Task<bool> AddPlayerAsync(PlayerDTO player);
        Task PutPlayerAsync(PlayerDTO player);
        Task<List<SessionResponse>> GetSessionsByPlayerAsync(string username);
        Task AddSessionToPlayerAsync(string username, int sessionId);
        Task RemoveSessionFromPlayerAsync(string username, int sessionId);

        Task AddGameToPlayerAsync(string username, int gameId);

        Task RemoveGameFromPlayerAsync(string username, int gameId);
    }
}
