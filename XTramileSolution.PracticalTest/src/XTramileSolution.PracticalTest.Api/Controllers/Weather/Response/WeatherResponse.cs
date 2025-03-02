using System;
using System.Collections.Generic;

namespace XTramileSolution.PracticalTest.Api.Controllers.Weather.Response
{
    public class WeatherLocationResponse
    {
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class WeatherWindResponse
    {
        public double Speed { get; set; } // In meters/second
        public int Degree { get; set; } // Wind direction in degrees
        public double Gust { get; set; } // Gust speed in meters/second
    }

    public class WeatherTemperatureResponse
    {
        public double Celsius { get; set; }
        public double Fahrenheit { get; set; }
        public double DewPoint { get; set; } // The dew point temperature
    }

    public class WeatherConditionResponse
    {
        public int Id { get; set; }

        public string Main { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }
    }
    public class WeatherTimeResponse
    {
        public DateTimeOffset LocalTime { get; set; }
        public int Offset { get; set; }
    }
    public class WeatherResponse
    {
        public WeatherLocationResponse WeatherLocationResponse { get; set; }
        public WeatherTimeResponse Time { get; set; }
        public WeatherWindResponse WeatherWindResponse { get; set; }
        public int Visibility { get; set; }
        public List<WeatherConditionResponse> SkyConditions { get; set; }
        public WeatherTemperatureResponse WeatherTemperatureResponse { get; set; }
        public int Humidity { get; set; } // Relative Humidity in percentage
        public int Pressure { get; set; } // Atmospheric pressure in hPa
    }
}