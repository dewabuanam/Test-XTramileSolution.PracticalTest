using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XTramileSolution.PracticalTest.Api.Controllers.Weather.Response;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.Interface;
using XTramileSolution.PracticalTest.Service.ResourceModel;

namespace XTramileSolution.PracticalTest.Api.Controllers.Weather
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{cityId}")]
        public async Task<ActionResult<WeatherResponse>> Get(Guid cityId)
        {
            var weather = await _weatherService.GetWeatherByCityAsync(cityId);
            if (weather == null) return NotFound();
            return await MapResponse(weather);
        }

        private Task<WeatherResponse> MapResponse(WeatherResourceModel weatherResourceModel)
        {
            return Task.FromResult(new WeatherResponse
            {
                WeatherLocationResponse = new WeatherLocationResponse()
                {
                    City = weatherResourceModel.Location.City,
                    Country = weatherResourceModel.Location.Country
                },
                Time = new WeatherTimeResponse()
                {
                    LocalTime = weatherResourceModel.Time.LocalTime,
                    Offset = weatherResourceModel.Time.Offset
                },
                WeatherWindResponse = new WeatherWindResponse()
                {
                    Speed = weatherResourceModel.Wind.Speed,
                    Degree = weatherResourceModel.Wind.Degree,
                    Gust = weatherResourceModel.Wind.Gust
                },
                Visibility = weatherResourceModel.Visibility,
                SkyConditions = weatherResourceModel.SkyConditions.Select(x=> new WeatherConditionResponse()
                {
                    Id = x.Id,
                    Main = x.Main,
                    Description = x.Description,
                    Icon = x.Icon
                }).ToList(),
                WeatherTemperatureResponse = new WeatherTemperatureResponse()
                {
                    Fahrenheit = weatherResourceModel.Temperature.Fahrenheit,
                    Celsius = weatherResourceModel.Temperature.Celsius,
                    DewPoint = weatherResourceModel.Temperature.DewPoint,
                },
                Humidity = weatherResourceModel.Humidity,
                Pressure = weatherResourceModel.Pressure
            });
        }
    }
}
