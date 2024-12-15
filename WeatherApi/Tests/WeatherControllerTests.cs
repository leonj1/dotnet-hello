using Microsoft.AspNetCore.Mvc;
using WeatherApi.Controllers;
using WeatherApi.Models;
using WeatherApi.Services;
using Xunit;

namespace WeatherApi.Tests;

public class WeatherControllerTests
{
    private static readonly DateTime CurrentTime = 
        DateTime.Parse("2024-12-15T00:46:49Z");

    public static IEnumerable<object[]> WeatherTestCases()
    {
        yield return new object[] 
        { 
            "London",
            new Weather("London", 22.5, 65, "Partly Cloudy", CurrentTime)
        };
        yield return new object[] 
        { 
            "Tokyo",
            new Weather("Tokyo", 22.5, 65, "Partly Cloudy", CurrentTime)
        };
    }

    [Theory]
    [MemberData(nameof(WeatherTestCases))]
    public async Task Get_ReturnsWeatherForRegion(
        string region, 
        Weather expectedWeather)
    {
        var strategy = new MockWeatherStrategy();
        var controller = new WeatherController(strategy);

        var result = await controller.Get(region);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var weather = Assert.IsType<Weather>(okResult.Value);

        Assert.Equal(expectedWeather, weather);
    }
}
