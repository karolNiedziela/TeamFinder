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
        Task<PlayerResponse> GetPlayerAsync(string username);
        Task<List<PlayerResponse>> GetPlayerGamesAsync(string username);
        Task<List<SessionResponse>> GetPlayerSessionsAsync(string username);
        Task<List<GameDTO>> GetGamesAsync();
        Task<GameDTO> GetGameAsync(string name);
    }
}
