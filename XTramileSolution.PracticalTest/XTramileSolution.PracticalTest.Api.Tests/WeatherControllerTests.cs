using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XTramileSolution.PracticalTest.Api.Controllers.Weather;
using XTramileSolution.PracticalTest.Api.Controllers.Weather.Response;
using XTramileSolution.PracticalTest.Service.Interface;
using XTramileSolution.PracticalTest.Service.ResourceModel;
using Xunit;

namespace XTramileSolution.PracticalTest.Api.Tests
{
    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly WeatherController _weatherController;

        public WeatherControllerTests()
        {
            _mockWeatherService = new Mock<IWeatherService>();
            _weatherController = new WeatherController(_mockWeatherService.Object);
        }

        // ✅ Test GET /api/weather/{cityId} - Return weather data
        [Fact]
        public async Task Get_ShouldReturnWeatherData_WhenCityExists()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            var weatherData = new WeatherResourceModel
            {
                Location = new WeatherLocationResourceModel { City = "Jakarta", Country = "Indonesia" },
                Time = new WeatherTimeResourceModel { LocalTime = DateTime.UtcNow, Offset = 7 },
                Wind = new WeatherWindResourceModel { Speed = 10, Degree = 180, Gust = 15 },
                Visibility = 10,
                SkyConditions = new List<WeatherConditionResourceModel>
                {
                    new WeatherConditionResourceModel { Id = 1, Main = "Clear", Description = "Sunny", Icon = "01d" }
                },
                Temperature = new WeatherTemperatureResourceModel { Fahrenheit = 86, Celsius = 30, DewPoint = 20 },
                Humidity = 80,
                Pressure = 1010
            };

            _mockWeatherService.Setup(s => s.GetWeatherByCityAsync(cityId))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _weatherController.Get(cityId);
            var actionResult = Assert.IsType<ActionResult<WeatherResponse>>(result);
            var response = Assert.IsType<WeatherResponse>(actionResult.Value);

            // Assert
            Assert.Equal("Jakarta", response.WeatherLocationResponse.City);
            Assert.Equal("Indonesia", response.WeatherLocationResponse.Country);
            Assert.Equal(30, response.WeatherTemperatureResponse.Celsius);
            Assert.Single(response.SkyConditions);
            Assert.Equal("Sunny", response.SkyConditions[0].Description);
        }

        // ✅ Test GET /api/weather/{cityId} - Return 404 if city not found
        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenCityDoesNotExist()
        {
            // Arrange
            var cityId = Guid.NewGuid();

            _mockWeatherService.Setup(s => s.GetWeatherByCityAsync(cityId))
                .ReturnsAsync((WeatherResourceModel)null);

            // Act
            var result = await _weatherController.Get(cityId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // ✅ Test GET /api/weather/{cityId} - Handle service exception
        [Fact]
        public async Task Get_ShouldHandleServiceException()
        {
            // Arrange
            var cityId = Guid.NewGuid();

            _mockWeatherService.Setup(s => s.GetWeatherByCityAsync(cityId))
                .ThrowsAsync(new Exception("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _weatherController.Get(cityId));
        }
    }
}
