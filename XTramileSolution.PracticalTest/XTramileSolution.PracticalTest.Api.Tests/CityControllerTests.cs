using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using XTramileSolution.PracticalTest.Api.Controllers.City;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.Interface;
using Xunit;

namespace XTramileSolution.PracticalTest.Api.Tests
{
    public class CityControllerTests
    {
        private readonly Mock<ICityService> _mockCityService;
        private readonly CityController _cityController;

        public CityControllerTests()
        {
            _mockCityService = new Mock<ICityService>();
            _cityController = new CityController(_mockCityService.Object);
        }

        [Fact]
        public async Task GetCountryCity_ShouldReturnCities_WhenCountryHasCities()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var cities = new List<CityResourceModel>
            {
                new CityResourceModel { Id = Guid.NewGuid(), CountryId = countryId, Name = "Jakarta" },
                new CityResourceModel { Id = Guid.NewGuid(), CountryId = countryId, Name = "Bandung" }
            };

            _mockCityService.Setup(s => s.GetAllCountryCityAsync(countryId))
                .ReturnsAsync(cities);

            // Act
            var result = await _cityController.GetCountryCity(countryId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Jakarta");
            Assert.Contains(result, c => c.Name == "Bandung");
        }

        [Fact]
        public async Task GetCountryCity_ShouldReturnEmpty_WhenNoCitiesForCountry()
        {
            // Arrange
            var countryId = Guid.NewGuid();

            _mockCityService.Setup(s => s.GetAllCountryCityAsync(countryId))
                .ReturnsAsync(new List<CityResourceModel>());

            // Act
            var result = await _cityController.GetCountryCity(countryId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCountryCity_ShouldReturnEmpty_WhenCountryDoesNotExist()
        {
            // Arrange
            var nonExistentCountryId = Guid.NewGuid();

            _mockCityService.Setup(s => s.GetAllCountryCityAsync(nonExistentCountryId))
                .ReturnsAsync(new List<CityResourceModel>());

            // Act
            var result = await _cityController.GetCountryCity(nonExistentCountryId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCountryCity_ShouldHandleServiceException()
        {
            // Arrange
            var countryId = Guid.NewGuid();

            _mockCityService.Setup(s => s.GetAllCountryCityAsync(countryId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _cityController.GetCountryCity(countryId));
        }
    }
}
