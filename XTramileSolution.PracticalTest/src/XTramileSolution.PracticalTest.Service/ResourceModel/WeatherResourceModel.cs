using System;
using System.Collections.Generic;

namespace XTramileSolution.PracticalTest.Service.ResourceModel
{
    public class WeatherLocationResourceModel
    {
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class WeatherWindResourceModel
    {
        public double Speed { get; set; } // In meters/second
        public int Degree { get; set; } // Wind direction in degrees
        public double Gust { get; set; } // Gust speed in meters/second
    }

    public class WeatherTemperatureResourceModel
    
    {
        public double Celsius { get; set; }
        public double Fahrenheit { get; set; }
        public double DewPoint { get; set; } // The dew point temperature
    }

    public class WeatherConditionResourceModel
    {
        public int Id { get; set; }

        public string Main { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }
    }


    public class WeatherTimeResourceModel
    {
        public DateTime LocalTime { get; set; }
        public int Offset { get; set; }
    }
    public class WeatherResourceModel
    {
        public WeatherLocationResourceModel Location { get; set; }
        public WeatherTimeResourceModel Time { get; set; }
        public WeatherWindResourceModel Wind { get; set; }
        public int Visibility { get; set; }
        public List<WeatherConditionResourceModel> SkyConditions { get; set; }
        public WeatherTemperatureResourceModel Temperature { get; set; }
        public int Humidity { get; set; } // Relative Humidity in percentage
        public int Pressure { get; set; } // Atmospheric pressure in hPa
    }
}