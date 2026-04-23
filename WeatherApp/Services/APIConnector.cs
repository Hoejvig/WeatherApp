using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class APIConnector
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public APIConnector(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherReading?> GetWeatherAsync(string city)
        {
            string apiKey = _configuration["WeatherStack:ApiKey"] ?? throw new InvalidOperationException("API key not configured.");
            string url = $"http://api.weatherstack.com/current?access_key={apiKey}&query={city}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("current", out var current))
            {
                return null;
            }

            string locationName = root.GetProperty("location").GetProperty("name").GetString() ?? city;
            double temperature = current.GetProperty("temperature").GetDouble();
            int humidity = current.GetProperty("humidity").GetInt32();
            double? windSpeed = null;
            if (current.TryGetProperty("wind_speed", out var wind))
                windSpeed = wind.GetDouble();
            double? precipitation = null;
            if (current.TryGetProperty("precip", out var precip))
                precipitation = precip.GetDouble();

            string description = "";
            if (current.TryGetProperty("weather_descriptions", out var descriptions) &&
                descriptions.GetArrayLength() > 0)
            {
                description = descriptions[0].GetString() ?? "";
            }

            return new WeatherReading
            {
                City = locationName,
                Temperature = temperature,
                Humidity = humidity,
                WindSpeed = windSpeed,
                Precipitation = precipitation,
                Description = description
            };
        }
    }
}