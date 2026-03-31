using System.Text.Json;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class APIConnector
    {
        private readonly HttpClient _httpClient;

        public APIConnector(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherViewModel?> GetWeatherAsync(string city)
        {
            string apiKey = "7a300817089e89fdb259c0c1d7f4c49a";
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

            string description = "";
            if (current.TryGetProperty("weather_descriptions", out var descriptions) &&
                descriptions.GetArrayLength() > 0)
            {
                description = descriptions[0].GetString() ?? "";
            }

            string iconUrl = "";
            if (current.TryGetProperty("weather_icons", out var icons) &&
                icons.GetArrayLength() > 0)
            {
                iconUrl = icons[0].GetString() ?? "";
            }

            return new WeatherViewModel
            {
                City = locationName,
                Temperature = temperature,
                Humidity = humidity,
                Description = description,
                IconUrl = iconUrl
            };
        }
    }
}