namespace WeatherApp.Models
{
    public class WeatherDashboardViewModel
    {
        public WeatherSnapshot? Latest { get; set; }

        public List<WeatherSnapshot> History { get; set; } = new();
    }
}