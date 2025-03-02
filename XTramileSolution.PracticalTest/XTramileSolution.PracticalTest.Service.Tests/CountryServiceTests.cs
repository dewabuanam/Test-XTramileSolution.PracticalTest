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
    public class CountryServiceTests
    {
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly CountryService _countryService;

        public CountryServiceTests()
        {
            _mockCountryRepository = new Mock<ICountryRepository>();
            _countryService = new CountryService(_mockCountryRepository.Object);
        }

        [Fact]
        public async Task GetAllCountriesAsync_Returns_Countries()
        {
            // Arrange
            var countries = new List<CountryEntity>
            {
                new CountryEntity { Id = Guid.NewGuid(), Name = "Indonesia", Code = "ID" },
                new CountryEntity { Id = Guid.NewGuid(), Name = "United States", Code = "US" }
            };

            _mockCountryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(countries);

            // Act
            var result = await _countryService.GetAllCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Indonesia");
            Assert.Contains(result, c => c.Code == "US");
        }

        [Fact]
        public async Task GetAllCountriesAsync_Returns_EmptyList_If_No_Countries()
        {
            // Arrange
            _mockCountryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<CountryEntity>());

            // Act
            var result = await _countryService.GetAllCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCountryByIdAsync_Returns_Correct_Country()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var country = new CountryEntity { Id = countryId, Name = "Japan", Code = "JP" };

            _mockCountryRepository.Setup(repo => repo.GetByIdAsync(countryId)).ReturnsAsync(country);

            // Act
            var result = await _countryService.GetCountryByIdAsync(countryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Japan", result.Name);
            Assert.Equal("JP", result.Code);
        }

        [Fact]
        public async Task GetCountryByIdAsync_Returns_Null_If_Not_Found()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            _mockCountryRepository.Setup(repo => repo.GetByIdAsync(countryId)).ReturnsAsync((CountryEntity)null);

            // Act
            var result = await _countryService.GetCountryByIdAsync(countryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCountriesAsync_Calls_Repository_Once()
        {
            // Arrange
            _mockCountryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<CountryEntity>());

            // Act
            await _countryService.GetAllCountriesAsync();

            // Assert
            _mockCountryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCountryByIdAsync_Calls_Repository_With_Correct_Id()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            _mockCountryRepository.Setup(repo => repo.GetByIdAsync(countryId)).ReturnsAsync(new CountryEntity());

            // Act
            await _countryService.GetCountryByIdAsync(countryId);

            // Assert
            _mockCountryRepository.Verify(repo => repo.GetByIdAsync(It.Is<Guid>(id => id == countryId)), Times.Once);
        }
    }
}
