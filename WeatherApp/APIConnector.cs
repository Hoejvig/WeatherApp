using System.Net.Http;
using System.Text.Json;

public class APIConnector
{
    private readonly HttpClient _httpClient;

    public APIConnector(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetWeatherRawAsync()
    {
        string apiKey = "YOUR_API_KEY";
        string city = "Copenhagen";
        string url = $"http://api.weatherstack.com/current?access_key={apiKey}&query={city}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}