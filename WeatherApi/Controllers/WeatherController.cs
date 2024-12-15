using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherStrategy _weatherStrategy;

    public WeatherController(IWeatherStrategy weatherStrategy)
    {
        _weatherStrategy = weatherStrategy;
    }

    [HttpGet("{region}")]
    public async Task<IActionResult> Get(string region)
    {
        if (string.IsNullOrWhiteSpace(region))
        {
            return NotFound();
        }

        try
        {
            var weather = await _weatherStrategy.GetWeatherAsync(region);
            return Ok(weather);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}
