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
using XTramileSolution.PracticalTest.Api.Controllers.City.Response;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.Interface;
using Xunit;

namespace XTramileSolution.PracticalTest.Api.IntegrationTests
{
    public class CityControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly Mock<ICityService> _mockCityService = new Mock<ICityService>();

        public CityControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove original ICityService registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICityService));
                    if (descriptor != null) services.Remove(descriptor);

                    // Add mock service
                    services.AddSingleton(_mockCityService.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetCountryCity_ShouldReturnCities_WhenCountryExists()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            var cities = new List<CityResourceModel>
            {
                new CityResourceModel() { Id = Guid.NewGuid(), CountryId = countryId, Name = "Jakarta" },
                new CityResourceModel() { Id = Guid.NewGuid(), CountryId = countryId, Name = "Surabaya" }
            };

            _mockCityService.Setup(s => s.GetAllCountryCityAsync(countryId)).ReturnsAsync(cities);

            // Act
            var response = await _client.GetAsync($"/api/cities/{countryId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<CityResponse>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Jakarta");
            Assert.Contains(result, c => c.Name == "Surabaya");
        }

        [Fact]
        public async Task GetCountryCity_ShouldReturnEmptyList_WhenNoCitiesExist()
        {
            // Arrange
            var countryId = Guid.NewGuid();
            _mockCityService.Setup(s => s.GetAllCountryCityAsync(countryId)).ReturnsAsync(new List<CityResourceModel>());

            // Act
            var response = await _client.GetAsync($"/api/cities/{countryId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<CityResponse>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
