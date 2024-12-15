namespace WeatherApi.Models;

public record Weather(
    string Region,
    double Temperature,
    int Humidity,
    string Condition,
    DateTime Timestamp
);
