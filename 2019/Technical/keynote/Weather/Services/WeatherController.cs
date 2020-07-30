using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Weather.Model;

namespace Weather.Services
{
    [ApiController]
    public class WeatherController : Controller
    {
        private IMemoryCache _cache;

        public WeatherController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [EnableCors()]
        [HttpGet("/json")]
        public ActionResult<WeatherResponse> GetWeatherForecast()
        {
            var forecast = _cache.Get<Forecast>(Constants.LATEST_FORECAST_CACHE_KEY);
            return WeatherService.GetCurrentWeatherResponse(forecast);
        }
    }
}
