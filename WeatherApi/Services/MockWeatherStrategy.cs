using WeatherApi.Models;

namespace WeatherApi.Services;

public class MockWeatherStrategy : IWeatherStrategy
{
    private static readonly DateTime CurrentTime = 
        DateTime.Parse("2024-12-15T01:19:41Z");

    public Task<Weather> GetWeatherAsync(string region)
    {
        if (string.IsNullOrWhiteSpace(region))
        {
            throw new ArgumentException("Region cannot be null or empty", nameof(region));
        }

        var weather = new Weather(
            Region: region,
            Temperature: 22.5,
            Humidity: 65,
            Condition: "Partly Cloudy",
            Timestamp: CurrentTime
        );

        return Task.FromResult(weather);
    }
}
