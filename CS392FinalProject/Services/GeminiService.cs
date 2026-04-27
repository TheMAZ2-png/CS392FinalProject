using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using CS392FinalProject.Models;

namespace CS392FinalProject.Services
{
    public class GeminiService
    {
        private readonly HttpClient _http;
        private readonly GeminiSettings _settings;

        public GeminiService(IOptions<GeminiSettings> settings)
        {
            _settings = settings.Value;

            _http = new HttpClient
            {
                BaseAddress = new Uri(_settings.BaseAddress)
            };
        }

        public async Task<string> AskGeminiAsync(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var url = $"{_settings.Model}:generateContent?key={_settings.ApiKey}";

            var response = await _http.PostAsJsonAsync(url, requestBody);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "";
        }
    }
}
