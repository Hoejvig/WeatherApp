using Microsoft.AspNetCore.Mvc;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class WeatherController : Controller
    {
        private readonly APIConnector _apiConnector;

        public WeatherController(APIConnector apiConnector)
        {
            _apiConnector = apiConnector;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _apiConnector.GetWeatherAsync("Copenhagen");

            if (model == null)
            {
                ViewBag.Error = "Could not load weather data.";
                return View();
            }

            return View(model);
        }
    }
}