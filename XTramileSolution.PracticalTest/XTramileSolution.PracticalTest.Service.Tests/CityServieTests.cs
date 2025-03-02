using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Service.Application;
using Xunit;

namespace XTramileSolution.PracticalTest.Service.Tests
{
    public class CityServiceTests
    {
        private readonly Mock<ICityRepository> _mockCityRepository;
        private readonly CityService _cityService;

        public CityServiceTests()
        {
            _mockCityRepository = new Mock<ICityRepository>();
            _cityService = new CityService(_mockCityRepository.Object);
        }

        [Fact]
        public async Task GetAllCountryCityAsync_Returns_Cities()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var cities = new List<CityEntity>
            {
                new CityEntity { Id = Guid.NewGuid(), CountryId = countryId, Name = "Jakarta" },
                new CityEntity { Id = Guid.NewGuid(), CountryId = countryId, Name = "Bali" }
            };

            _mockCityRepository.Setup(repo => repo.GetAllCountryCityAsync(countryId)).ReturnsAsync(cities);

            // Act
            var result = await _cityService.GetAllCountryCityAsync(countryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Jakarta");
            Assert.Contains(result, c => c.Name == "Bali");
        }

        [Fact]
        public async Task GetAllCountryCityAsync_Returns_EmptyList_If_No_Cities()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            _mockCityRepository.Setup(repo => repo.GetAllCountryCityAsync(countryId)).ReturnsAsync(new List<CityEntity>());

            // Act
            var result = await _cityService.GetAllCountryCityAsync(countryId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCityByIdAsync_Returns_Correct_City()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            var city = new CityEntity { Id = cityId, CountryId = Guid.NewGuid(), Name = "Tokyo" };

            _mockCityRepository.Setup(repo => repo.GetByIdAsync(cityId)).ReturnsAsync(city);

            // Act
            var result = await _cityService.GetCityByIdAsync(cityId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tokyo", result.Name);
        }

        [Fact]
        public async Task GetCityByIdAsync_Returns_Null_If_Not_Found()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            _mockCityRepository.Setup(repo => repo.GetByIdAsync(cityId)).ReturnsAsync((CityEntity)null);

            // Act
            var result = await _cityService.GetCityByIdAsync(cityId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCountryCityAsync_Calls_Repository_Once()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            _mockCityRepository.Setup(repo => repo.GetAllCountryCityAsync(countryId)).ReturnsAsync(new List<CityEntity>());

            // Act
            await _cityService.GetAllCountryCityAsync(countryId);

            // Assert
            _mockCityRepository.Verify(repo => repo.GetAllCountryCityAsync(countryId), Times.Once);
        }

        [Fact]
        public async Task GetCityByIdAsync_Calls_Repository_With_Correct_Id()
        {
            // Arrange
            var cityId = Guid.NewGuid();
            _mockCityRepository.Setup(repo => repo.GetByIdAsync(cityId)).ReturnsAsync(new CityEntity());

            // Act
            await _cityService.GetCityByIdAsync(cityId);

            // Assert
            _mockCityRepository.Verify(repo => repo.GetByIdAsync(It.Is<Guid>(id => id == cityId)), Times.Once);
        }
    }
}
