using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XTramileSolution.PracticalTest.Api.Controllers.Country;
using XTramileSolution.PracticalTest.Api.Controllers.Country.Response;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.Interface;
using Xunit;

namespace XTramileSolution.PracticalTest.Api.Tests
{
    public class CountryControllerTests
    {
        private readonly Mock<ICountryService> _mockCountryService;
        private readonly CountryController _countryController;

        public CountryControllerTests()
        {
            _mockCountryService = new Mock<ICountryService>();
            _countryController = new CountryController(_mockCountryService.Object);
        }

        // ✅ Test Get (GET /api/countries) - Return countries list
        [Fact]
        public async Task Get_ShouldReturnCountries_WhenDataExists()
        {
            // Arrange
            var countries = new List<CountryResourceModel>
            {
                new CountryResourceModel { Id = Guid.NewGuid(), Name = "Indonesia", Code = "ID" },
                new CountryResourceModel { Id = Guid.NewGuid(), Name = "United States", Code = "US" }
            };

            _mockCountryService.Setup(s => s.GetAllCountriesAsync())
                .ReturnsAsync(countries);

            // Act
            var result = await _countryController.Get();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Indonesia");
            Assert.Contains(result, c => c.Name == "United States");
        }

        // ✅ Test Get (GET /api/countries) - Return empty list
        [Fact]
        public async Task Get_ShouldReturnEmpty_WhenNoCountriesExist()
        {
            // Arrange
            _mockCountryService.Setup(s => s.GetAllCountriesAsync())
                .ReturnsAsync(new List<CountryResourceModel>());

            // Act
            var result = await _countryController.Get();

            // Assert
            Assert.Empty(result);
        }

        // ✅ Test Get By Id (GET /api/countries/{id}) - Return country details
        [Fact]
        public async Task GetById_ShouldReturnCountry_WhenCountryExists()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var country = new CountryResourceModel { Id = countryId, Name = "Indonesia", Code = "ID" };

            _mockCountryService.Setup(s => s.GetCountryByIdAsync(countryId))
                .ReturnsAsync(country);

            // Act
            var result = await _countryController.Get(countryId);
            var actionResult = Assert.IsType<ActionResult<CountryResponse>>(result);
            var response = Assert.IsType<CountryResponse>(actionResult.Value);

            // Assert
            Assert.Equal("Indonesia", response.Name);
            Assert.Equal("ID", response.Code);
        }

        // ✅ Test Get By Id (GET /api/countries/{id}) - Return 404 if country not found
        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            _mockCountryService.Setup(s => s.GetCountryByIdAsync(nonExistentId))
                .ReturnsAsync((CountryResourceModel)null);

            // Act
            var result = await _countryController.Get(nonExistentId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // ✅ Test Get (GET /api/countries) - Handle service exception
        [Fact]
        public async Task Get_ShouldHandleServiceException()
        {
            // Arrange
            _mockCountryService.Setup(s => s.GetAllCountriesAsync())
                .ThrowsAsync(new Exception("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _countryController.Get());
        }

        // ✅ Test Get By Id (GET /api/countries/{id}) - Handle service exception
        [Fact]
        public async Task GetById_ShouldHandleServiceException()
        {
            // Arrange
            var countryId = Guid.NewGuid();

            _mockCountryService.Setup(s => s.GetCountryByIdAsync(countryId))
                .ThrowsAsync(new Exception("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _countryController.Get(countryId));
        }
    }
}
