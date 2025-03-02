using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.Helper;
using XTramileSolution.PracticalTest.Service.Interface;
using XTramileSolution.PracticalTest.Service.ResourceModel;
using XTramileSolution.PracticalTest.Service.ServiceModel;

namespace XTramileSolution.PracticalTest.Service.Application
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public async Task<WeatherResourceModel> GetWeatherByCityAsync(Guid cityId)
        {
            return await MapToWeatherResourceModel(await _weatherRepository.GetWeatherByCityAsync(cityId));
        }

        private async Task<WeatherResourceModel> MapToWeatherResourceModel(WeatherEntity weatherEntity)
        {
            return new WeatherResourceModel
            {
                Location = new WeatherLocationResourceModel()
                {
                    City = weatherEntity.Location.City,
                    Country = weatherEntity.Location.Country
                },
                Time = new WeatherTimeResourceModel()
                {
                    LocalTime = weatherEntity.Time.UtcTime,
                    Offset = weatherEntity.Time.Offset
                },
                Wind = new WeatherWindResourceModel()
                {
                    Speed = weatherEntity.Wind.Speed,
                    Degree = weatherEntity.Wind.Degree,
                    Gust = weatherEntity.Wind.Gust
                },
                Visibility = weatherEntity.Visibility,
                SkyConditions = weatherEntity.SkyConditions.Select(x=> new WeatherConditionResourceModel()
                {
                    Id = x.Id,
                    Main = x.Main,
                    Description = x.Description,
                    Icon = x.Icon
                }).ToList(),
                Temperature = new WeatherTemperatureResourceModel()
                {
                    Fahrenheit = weatherEntity.Temperature.Fahrenheit,
                    Celsius = TemperatureHelper.ConvertFahrenheitToCelsius(weatherEntity.Temperature.Fahrenheit),
                    DewPoint = weatherEntity.Temperature.DewPoint
                },
                Humidity = weatherEntity.Humidity,
                Pressure = weatherEntity.Pressure
            };
        }
    }
}
