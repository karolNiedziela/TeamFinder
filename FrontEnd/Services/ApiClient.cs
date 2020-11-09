using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TeamFinderDTO;

namespace FrontEnd.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GameDTO> GetGameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"/api/games/{name}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<GameDTO>();
        }

        public async Task<List<GameDTO>> GetGamesAsync()
        {
            var response = await _httpClient.GetAsync($"/api/games");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<GameDTO>>();
        }

        public async Task<PlayerResponse> GetPlayerAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"/api/players/{username}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<PlayerResponse>();
        }

        public async Task<List<PlayerResponse>> GetPlayerGamesAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"/api/players/{username}/games");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<PlayerResponse>>();
        }

        public async Task<List<PlayerResponse>> GetPlayersAsync()
        {
            var response = await _httpClient.GetAsync($"/api/players");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<PlayerResponse>>();
        }

        public async Task<List<SessionResponse>> GetPlayerSessionsAsync(string username)
        {
            if(string.IsNullOrEmpty(username))
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"/api/player/{username}/sessions");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SessionResponse>>();
        }

        public async Task<SessionResponse> GetSessionAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/sessions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<SessionResponse>();
        }

        public Task<List<SessionResponse>> GetSessionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
