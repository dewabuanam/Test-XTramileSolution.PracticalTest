using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Service.Application;
using XTramileSolution.PracticalTest.Service.Helper;
using Xunit;

namespace XTramileSolution.PracticalTest.Service.Tests
{
    public class WeatherServiceTests
    {
        private readonly Mock<IWeatherRepository> _mockWeatherRepository;
        private readonly WeatherService _weatherService;

        public WeatherServiceTests()
        {
            _mockWeatherRepository = new Mock<IWeatherRepository>();
            _weatherService = new WeatherService(_mockWeatherRepository.Object);
        }

        [Fact]
        public async Task GetWeatherByCityAsync_Returns_WeatherData()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            var weatherEntity = GetMockWeatherEntity();

            _mockWeatherRepository.Setup(repo => repo.GetWeatherByCityAsync(cityId))
                .ReturnsAsync(weatherEntity);

            // Act
            var result = await _weatherService.GetWeatherByCityAsync(cityId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jakarta", result.Location.City);
            Assert.Equal("Indonesia", result.Location.Country);
            Assert.Equal(20.0, result.Temperature.Celsius, 1);  // Allow small floating-point deviation
        }

        [Fact]
        public async Task GetWeatherByCityAsync_Correctly_Maps_Weather_Properties()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            var weatherEntity = GetMockWeatherEntity();

            _mockWeatherRepository.Setup(repo => repo.GetWeatherByCityAsync(cityId))
                .ReturnsAsync(weatherEntity);

            // Act
            var result = await _weatherService.GetWeatherByCityAsync(cityId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(weatherEntity.Visibility, result.Visibility);
            Assert.Equal(weatherEntity.Humidity, result.Humidity);
            Assert.Equal(weatherEntity.Pressure, result.Pressure);
            Assert.Equal(weatherEntity.Wind.Speed, result.Wind.Speed);
        }

        [Fact]
        public async Task GetWeatherByCityAsync_Converts_Temperature_Correctly()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            var weatherEntity = GetMockWeatherEntity();
            _mockWeatherRepository.Setup(repo => repo.GetWeatherByCityAsync(cityId))
                .ReturnsAsync(weatherEntity);

            // Act
            var result = await _weatherService.GetWeatherByCityAsync(cityId);

            // Assert
            var expectedCelsius = TemperatureHelper.ConvertFahrenheitToCelsius(weatherEntity.Temperature.Fahrenheit);
            Assert.Equal(expectedCelsius, result.Temperature.Celsius, 1);
        }

        [Fact]
        public async Task GetWeatherByCityAsync_Calls_Repository_Once()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            _mockWeatherRepository.Setup(repo => repo.GetWeatherByCityAsync(cityId))
                .ReturnsAsync(GetMockWeatherEntity());

            // Act
            await _weatherService.GetWeatherByCityAsync(cityId);

            // Assert
            _mockWeatherRepository.Verify(repo => repo.GetWeatherByCityAsync(cityId), Times.Once);
        }

        [Fact]
        public async Task GetWeatherByCityAsync_Calls_Repository_With_Correct_CityId()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            _mockWeatherRepository.Setup(repo => repo.GetWeatherByCityAsync(cityId))
                .ReturnsAsync(GetMockWeatherEntity());

            // Act
            await _weatherService.GetWeatherByCityAsync(cityId);

            // Assert
            _mockWeatherRepository.Verify(repo => repo.GetWeatherByCityAsync(It.Is<Guid>(id => id == cityId)), Times.Once);
        }

        private WeatherEntity GetMockWeatherEntity()
        {
            return new WeatherEntity
            {
                Location = new WeatherLocationEntity
                {
                    City = "Jakarta",
                    Country = "Indonesia"
                },
                Time = new WeatherTimeEntity
                {
                    UtcTime = DateTime.UtcNow,
                    Offset = 7
                },
                Wind = new WeatherWindEntity
                {
                    Speed = 15,
                    Degree = 200,
                    Gust = 20
                },
                Visibility = 10,
                SkyConditions = new List<WeatherConditionEntity>
                {
                    new WeatherConditionEntity { Id = 1, Main = "Clear", Description = "Sunny", Icon = "01d" }
                },
                Temperature = new WeatherTemperatureEntity
                {
                    Fahrenheit = 68.0, // 20°C
                    DewPoint = 60.0
                },
                Humidity = 80,
                Pressure = 1013
            };
        }
    }
}
