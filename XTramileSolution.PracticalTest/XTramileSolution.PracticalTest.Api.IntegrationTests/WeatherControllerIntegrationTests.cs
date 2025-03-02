using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using XTramileSolution.PracticalTest.Api.Controllers.Weather.Response;
using XTramileSolution.PracticalTest.Service.Interface;
using XTramileSolution.PracticalTest.Service.ResourceModel;
using Xunit;

namespace XTramileSolution.PracticalTest.Api.IntegrationTests
{
    public class WeatherControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IWeatherService> _mockWeatherService = new Mock<IWeatherService>();

        public WeatherControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing IWeatherService
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IWeatherService));
                    if (descriptor != null) services.Remove(descriptor);

                    // Inject mocked IWeatherService
                    services.AddSingleton(_mockWeatherService.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetWeatherByCity_Returns_Weather_WhenFound()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            var mockWeather = new WeatherResourceModel
            {
                Location = new WeatherLocationResourceModel { City = "Tokyo", Country = "Japan" },
                Time = new WeatherTimeResourceModel { LocalTime = DateTime.UtcNow, Offset = 9 },
                Wind = new WeatherWindResourceModel { Speed = 10.5, Degree = 180, Gust = 15.2 },
                Visibility = 10000,
                SkyConditions = new List<WeatherConditionResourceModel>
                {
                    new WeatherConditionResourceModel { Id = 1, Main = "Clear", Description = "Clear sky", Icon = "01d" }
                },
                Temperature = new WeatherTemperatureResourceModel { Fahrenheit = 75.2, Celsius = 24, DewPoint = 18.5 },
                Humidity = 65,
                Pressure = 1013
            };

            _mockWeatherService.Setup(s => s.GetWeatherByCityAsync(cityId)).ReturnsAsync(mockWeather);

            // Act
            var response = await _client.GetAsync($"/api/weather/{cityId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WeatherResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("Tokyo", result.WeatherLocationResponse.City);
            Assert.Equal("Japan", result.WeatherLocationResponse.Country);
            Assert.Equal(24, result.WeatherTemperatureResponse.Celsius);
            Assert.Single(result.SkyConditions);
            Assert.Equal("Clear", result.SkyConditions[0].Main);
        }

        [Fact]
        public async Task GetWeatherByCity_Returns_NotFound_WhenNoWeatherExists()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            _mockWeatherService.Setup(s => s.GetWeatherByCityAsync(cityId)).ReturnsAsync((WeatherResourceModel)null);

            // Act
            var response = await _client.GetAsync($"/api/weather/{cityId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetWeatherByCity_Returns_BadRequest_ForInvalidId()
        {
            // Act
            var response = await _client.GetAsync("/api/weather/invalid-guid");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
