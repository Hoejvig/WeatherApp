namespace WeatherApp.Models
{
    public class WeatherViewModel
    {
        public string City { get; set; } = "";
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string Description { get; set; } = "";
        public string IconUrl { get; set; } = "";
    }
}