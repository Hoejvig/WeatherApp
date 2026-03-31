namespace WebApplication.Models
{
    public class WeatherSnapshot
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string City { get; set; } = "";

        public double Temperature { get; set; }

        public int Humidity { get; set; }

        public double? WindSpeed { get; set; }

        public string Description { get; set; } = "";
    }
}