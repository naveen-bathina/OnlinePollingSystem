using PollingApp.Models;
using System.Net.Http.Json;

namespace PollingApp.Services
{

    public interface IPollApiService
    {
        Task<List<PollDto>> GetPollsAsync();
        Task<PollDto> GetPollAsync(int pollId);
        Task SubmitVoteAsync(int pollId, CastVoteModel vote);
        Task<PollDto> GetPollResultAsync(int pollId);

    }

    public class PollApiService : IPollApiService
    {
        private readonly HttpClient _httpClient;

        public PollApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(MauiProgram.API_BASE_URL);
            var deviceId = SecureStorage.GetAsync("Device-Id").Result;
            _httpClient.DefaultRequestHeaders.Add("Device-Id", deviceId);
        }

        public async Task<List<PollDto>> GetPollsAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<PollDto>>("api/polls");
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PollDto> GetPollAsync(int pollId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<PollDto>($"api/polls/{pollId}");
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SubmitVoteAsync(int pollId, CastVoteModel vote)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/polls/{pollId}/vote", vote);
            response.EnsureSuccessStatusCode();
        }

        public async Task<PollDto> GetPollResultAsync(int pollId)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<PollDto>($"api/polls/{pollId}/results");
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
