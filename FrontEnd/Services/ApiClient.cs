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

        public async Task<PlayerResponse> GetPlayerAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/players/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<PlayerResponse>();
        }

        public async Task<List<PlayerResponse>> GetPlayerGamesAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/players/{id}/games");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<PlayerResponse>>();
        }

        public async Task<List<PlayerResponse>> GetPlayersAsync()
        {
            var response = await _httpClient.GetAsync($"/api/players");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<PlayerResponse>>();
        }

        public async Task<List<SessionResponse>> GetPlayerSessionsAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/player/{id}/sessions");

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

        public async Task<List<SessionResponse>> GetSessionsAsync()
        {

            var response = await _httpClient.GetAsync($"/api/sessions");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SessionResponse>>();
        }

       
        public async Task<List<SearchResult>> SearchAsync(string query)
        {
            var term = new SearchTerm
            {
                Query = query
            };

            var response = await _httpClient.PostAsJsonAsync($"/api/search", term);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<SearchResult>>();
        }

        public async Task<SessionResponse> PostSession(SessionDTO session)
        {
            throw new NotImplementedException();
        }

        public async Task PutSessionAsync(SessionDTO session)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/sessions/{session.Id}", session);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteSessionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/sessions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
