using WeatherApi.Models;

namespace WeatherApi.Services;

public interface IWeatherStrategy
{
    Task<Weather> GetWeatherAsync(string region);
}
