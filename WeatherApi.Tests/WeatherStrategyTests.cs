using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Tests;

public class WeatherStrategyTests
{
    private static readonly DateTime CurrentTime = DateTime.Parse("2024-12-15T01:19:41Z");

    public static IEnumerable<object[]> ValidWeatherStrategyTestCases()
    {
        yield return new object[]
        {
            "London",
            new Weather("London", 22.5, 65, "Partly Cloudy", CurrentTime),
            "Returns weather for London"
        };

        yield return new object[]
        {
            "São Paulo",
            new Weather("São Paulo", 22.5, 65, "Partly Cloudy", CurrentTime),
            "Handles special characters in city names"
        };

        yield return new object[]
        {
            "los-angeles",
            new Weather("los-angeles", 22.5, 65, "Partly Cloudy", CurrentTime),
            "Handles hyphenated city names"
        };
    }

    public static IEnumerable<object[]> InvalidWeatherStrategyTestCases()
    {
        yield return new object[]
        {
            "",
            "Empty region throws ArgumentException"
        };

        yield return new object[]
        {
            " ",
            "Whitespace region throws ArgumentException"
        };

        yield return new object[]
        {
            null!,
            "Null region throws ArgumentException"
        };
    }

    [Theory]
    [MemberData(nameof(ValidWeatherStrategyTestCases))]
    public async Task GetWeatherAsync_ValidRegion_ReturnsExpectedWeather(
        string region,
        Weather expectedWeather,
        string testDescription)
    {
        // Arrange
        var strategy = new MockWeatherStrategy();

        // Act
        var result = await strategy.GetWeatherAsync(region);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedWeather.Region, result.Region);
        Assert.Equal(expectedWeather.Temperature, result.Temperature);
        Assert.Equal(expectedWeather.Humidity, result.Humidity);
        Assert.Equal(expectedWeather.Condition, result.Condition);
    }

    [Theory]
    [MemberData(nameof(InvalidWeatherStrategyTestCases))]
    public async Task GetWeatherAsync_InvalidRegion_ThrowsArgumentException(
        string region,
        string testDescription)
    {
        // Arrange
        var strategy = new MockWeatherStrategy();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => strategy.GetWeatherAsync(region));
    }
}
