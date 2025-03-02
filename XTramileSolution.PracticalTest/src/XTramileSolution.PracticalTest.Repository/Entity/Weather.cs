using System;
using System.Collections.Generic;

namespace XTramileSolution.PracticalTest.Repository.Entity
{
    public class WeatherLocationEntity
    {
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class WeatherWindEntity
    {
        public double Speed { get; set; } // In meters/second
        public int Degree { get; set; } // Wind direction in degrees
        public double Gust { get; set; } // Gust speed in meters/second
    }

    public class WeatherTemperatureEntity
    {
        public double Fahrenheit { get; set; }
        public double DewPoint { get; set; } // The dew point temperature
    }

    public class WeatherConditionEntity
    {
        public int Id { get; set; }

        public string Main { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }
    }

    public class WeatherTimeEntity
    {
        public DateTime UtcTime { get; set; }
        public int Offset { get; set; }
    }

    public class WeatherEntity
    {
        public WeatherLocationEntity Location { get; set; }
        public WeatherTimeEntity Time { get; set; }
        public WeatherWindEntity Wind { get; set; }
        public int Visibility { get; set; }
        public List<WeatherConditionEntity> SkyConditions { get; set; }
        public WeatherTemperatureEntity Temperature { get; set; }
        public int Humidity { get; set; } // Relative Humidity in percentage
        public int Pressure { get; set; } // Atmospheric pressure in hPa
    }
}