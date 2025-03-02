using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using XTramileSolution.PracticalTest.Repository.Domain;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Repository.JsonResponse;

namespace XTramileSolution.PracticalTest.Repository.ThirdPartyApi
{
    public class OpenWeatherRepository : IWeatherRepository
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherApiOptions _weatherApiOptions;
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;

        public OpenWeatherRepository(ICountryRepository countryRepository, ICityRepository cityRepository, IOptions<WeatherApiOptions> weatherApiOptions)
        {
            _httpClient = new HttpClient();
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _weatherApiOptions = weatherApiOptions.Value;
        }

        public async Task<WeatherEntity> GetWeatherByCityAsync(Guid cityId)
        {
            var city = await _cityRepository.GetByIdAsync(cityId);
            string url = $"{_weatherApiOptions.BaseUrl}?q={city.Name}&appid={_weatherApiOptions.ApiKey}&units=imperial"; // Request data in Fahrenheit
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch weather data. Status Code: {response.StatusCode}");
            }

            string json = await response.Content.ReadAsStringAsync();
            var weatherResponse = JsonSerializer.Deserialize<OpenWeatherResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return await MapToWeatherToEntity(weatherResponse, city.CountryId);
        }

        private async Task<WeatherEntity> MapToWeatherToEntity(OpenWeatherResponse openWeatherResponse, Guid countryId)
        {
            var country = await _countryRepository.GetByIdAsync(countryId);
            return new WeatherEntity
            {
                Location = new WeatherLocationEntity()
                {
                    City = openWeatherResponse.Name,
                    Country = country.Name
                },
                Time = new WeatherTimeEntity()
                {
                    UtcTime = DateTime.UtcNow.AddSeconds(openWeatherResponse.Timezone),
                    Offset = openWeatherResponse.Timezone/3600
                },
                Wind = new WeatherWindEntity()
                {
                    Speed = openWeatherResponse.OpenWeatherWind.Speed,
                    Degree = openWeatherResponse.OpenWeatherWind.Deg,
                    Gust = openWeatherResponse.OpenWeatherWind.Gust
                },
                Visibility = openWeatherResponse.Visibility,
                SkyConditions = openWeatherResponse.Weather.Select(x=> new WeatherConditionEntity()
                {
                    Id = x.Id,
                    Main = x.Main,
                    Description = x.Description,
                    Icon = x.Icon
                }).ToList(),
                Temperature = new WeatherTemperatureEntity()
                {
                    Fahrenheit = openWeatherResponse.OpenWeatherMain.Temp,
                    DewPoint = openWeatherResponse.OpenWeatherMain.Temp - openWeatherResponse.OpenWeatherMain.FeelsLike
                },
                Humidity = openWeatherResponse.OpenWeatherMain.Humidity,
                Pressure = openWeatherResponse.OpenWeatherMain.Pressure
            };
        }
    }
}
