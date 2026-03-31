namespace WeatherApp.Models
{
    public class WeatherReading
    {
        public string City { get; set; } = "";
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double? WindSpeed { get; set; }
        public string Description { get; set; } = "";
    }
}