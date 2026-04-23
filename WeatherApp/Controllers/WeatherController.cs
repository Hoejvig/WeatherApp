using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Data;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherDbContext _db;

        public WeatherController(WeatherDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var history = await _db.WeatherSnapshots
                .OrderBy(x => x.Timestamp)
                .Take(200)
                .ToListAsync();

            var latest = history.LastOrDefault();

            var model = new WeatherDashboardViewModel
            {
                Latest = latest,
                History = history
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LatestPoint()
        {
            var latest = await _db.WeatherSnapshots
                .OrderByDescending(x => x.Timestamp)
                .Select(x => new
                {
                    time = x.Timestamp.ToString("HH:mm"),
                    temperature = x.Temperature,
                    precipitation = x.Precipitation
                })
                .FirstOrDefaultAsync();

            if (latest == null)
                return Json(new { });

            return Json(latest);
        }
    }
}