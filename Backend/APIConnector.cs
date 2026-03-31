using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var url = "https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=55.68&lon=12.57";

        using var client = new HttpClient();

        // REQUIRED by Met.no
        client.DefaultRequestHeaders.Add("User-Agent", "WeatherApp example@example.com");

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(json);

            var root = doc.RootElement;

            var timeseries = root
                .GetProperty("properties")
                .GetProperty("timeseries");

            var firstEntry = timeseries[0];

            var details = firstEntry
                .GetProperty("data")
                .GetProperty("instant")
                .GetProperty("details");

            double temperature = details.GetProperty("air_temperature").GetDouble();
            double humidity = details.GetProperty("relative_humidity").GetDouble();

            Console.WriteLine($"Temperature: {temperature} °C");
            Console.WriteLine($"Humidity: {humidity} %");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}