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
using XTramileSolution.PracticalTest.Api.Controllers.Country.Response;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.Interface;
using Xunit;

namespace XTramileSolution.PracticalTest.Api.IntegrationTests
{
    public class CountryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<ICountryService> _mockCountryService = new Mock<ICountryService>();

        public CountryControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing ICountryService
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICountryService));
                    if (descriptor != null) services.Remove(descriptor);

                    // Inject mocked ICountryService
                    services.AddSingleton(_mockCountryService.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetAllCountries_Returns_CountriesList()
        {
            // Arrange
            var mockCountries = new List<CountryResourceModel>
            {
                new CountryResourceModel { Id = Guid.NewGuid(), Name = "Indonesia", Code = "ID" },
                new CountryResourceModel { Id = Guid.NewGuid(), Name = "United States", Code = "US" }
            };

            _mockCountryService.Setup(s => s.GetAllCountriesAsync()).ReturnsAsync(mockCountries);

            // Act
            var response = await _client.GetAsync("/api/countries");
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<CountryResponse>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Indonesia", result[0].Name);
            Assert.Equal("United States", result[1].Name);
        }

        [Fact]
        public async Task GetCountryById_Returns_Country_WhenFound()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var mockCountry = new CountryResourceModel { Id = countryId, Name = "Japan", Code = "JP" };

            _mockCountryService.Setup(s => s.GetCountryByIdAsync(countryId)).ReturnsAsync(mockCountry);

            // Act
            var response = await _client.GetAsync($"/api/countries/{countryId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CountryResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("Japan", result.Name);
            Assert.Equal("JP", result.Code);
        }

        [Fact]
        public async Task GetCountryById_Returns_NotFound_WhenNoCountryExists()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            _mockCountryService.Setup(s => s.GetCountryByIdAsync(countryId)).ReturnsAsync((CountryResourceModel)null);

            // Act
            var response = await _client.GetAsync($"/api/countries/{countryId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
