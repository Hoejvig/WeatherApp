using WeatherApp.Data;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherPollingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WeatherPollingService> _logger;

        public WeatherPollingService(IServiceScopeFactory scopeFactory, ILogger<WeatherPollingService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            await SaveSnapshotAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                await SaveSnapshotAsync(stoppingToken);
            }
        }

        private async Task SaveSnapshotAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var api = scope.ServiceProvider.GetRequiredService<APIConnector>();
                var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

                var reading = await api.GetWeatherAsync("Copenhagen");
                if (reading == null)
                    return;

                var snapshot = new WeatherSnapshot
                {
                    Timestamp = DateTime.Now,
                    City = reading.City,
                    Temperature = reading.Temperature,
                    Humidity = reading.Humidity,
                    WindSpeed = reading.WindSpeed,
                    Description = reading.Description
                };

                db.WeatherSnapshots.Add(snapshot);
                await db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Saved weather snapshot at {Time}", snapshot.Timestamp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save weather snapshot.");
            }
        }
    }
}