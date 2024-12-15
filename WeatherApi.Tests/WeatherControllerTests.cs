using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApi.Controllers;
using WeatherApi.Models;
using WeatherApi.Services;
using Xunit;
using Xunit.Abstractions;

namespace WeatherApi.Tests;

[Collection("WeatherApi Tests")]
public class WeatherControllerTests
{
    private readonly Mock<ILogger<WeatherController>> _logger;
    private readonly ITestOutputHelper _output;

    public WeatherControllerTests(ITestOutputHelper output)
    {
        _logger = new Mock<ILogger<WeatherController>>();
        _output = output;
    }

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
        _output.WriteLine($"Running test: {testDescription}");
        var mockStrategy = new Mock<IWeatherStrategy>();
        mockStrategy.Setup(s => s.GetWeatherAsync(region))
            .ReturnsAsync(expectedWeather);

        var controller = new WeatherController(_logger.Object, mockStrategy.Object);

        // Act
        var result = await controller.Get(region);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedStatusCode, okResult.StatusCode);
        var weather = Assert.IsType<Weather>(okResult.Value);
        Assert.Equal(expectedWeather, weather);
    }

    [Theory]
    [MemberData(nameof(InvalidWeatherTestCases))]
    public async Task Get_InvalidRegion_ReturnsNotFound(
        string region,
        int expectedStatusCode,
        string testDescription)
    {
        // Arrange
        _output.WriteLine($"Running test: {testDescription}");
        var mockStrategy = new Mock<IWeatherStrategy>();
        mockStrategy.Setup(s => s.GetWeatherAsync(region))
            .ThrowsAsync(new ArgumentException("Invalid region"));

        var controller = new WeatherController(_logger.Object, mockStrategy.Object);

        // Act
        var result = await controller.Get(region);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
    }
}
