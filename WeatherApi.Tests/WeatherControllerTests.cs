using Microsoft.AspNetCore.Mvc;
using WeatherApi.Controllers;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Tests;

public class WeatherControllerTests
{
    private static readonly DateTime CurrentTime = DateTime.Parse("2024-12-15T01:19:41Z");

    public static IEnumerable<object[]> ValidWeatherTestCases()
    {
        yield return new object[]
        {
            "London",
            new Weather("London", 22.5, 65, "Partly Cloudy", CurrentTime),
            200,
            "Returns weather for London"
        };

        yield return new object[]
        {
            "Tokyo",
            new Weather("Tokyo", 22.5, 65, "Partly Cloudy", CurrentTime),
            200,
            "Returns weather for Tokyo"
        };

        yield return new object[]
        {
            "new-york",
            new Weather("new-york", 22.5, 65, "Partly Cloudy", CurrentTime),
            200,
            "Handles hyphenated city names"
        };
    }

    public static IEnumerable<object[]> InvalidWeatherTestCases()
    {
        yield return new object[]
        {
            "",
            404,
            "Empty region returns NotFound"
        };

        yield return new object[]
        {
            " ",
            404,
            "Whitespace region returns NotFound"
        };

        yield return new object[]
        {
            null!,
            404,
            "Null region returns NotFound"
        };
    }

    [Theory]
    [MemberData(nameof(ValidWeatherTestCases))]
    public async Task Get_ValidRegion_ReturnsExpectedWeather(
        string region,
        Weather expectedWeather,
        int expectedStatusCode,
        string testDescription)
    {
        // Arrange
        var strategy = new MockWeatherStrategy();
        var controller = new WeatherController(strategy);

        // Act
        var result = await controller.Get(region);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedStatusCode, objectResult.StatusCode);

        var weather = Assert.IsType<Weather>(objectResult.Value);
        Assert.Equal(expectedWeather.Region, weather.Region);
        Assert.Equal(expectedWeather.Temperature, weather.Temperature);
        Assert.Equal(expectedWeather.Humidity, weather.Humidity);
        Assert.Equal(expectedWeather.Condition, weather.Condition);
    }

    [Theory]
    [MemberData(nameof(InvalidWeatherTestCases))]
    public async Task Get_InvalidRegion_ReturnsNotFound(
        string region,
        int expectedStatusCode,
        string testDescription)
    {
        // Arrange
        var strategy = new MockWeatherStrategy();
        var controller = new WeatherController(strategy);

        // Act
        var result = await controller.Get(region);

        // Assert
        var objectResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(expectedStatusCode, objectResult.StatusCode);
    }
}
