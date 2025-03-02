using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Repository.JsonResponse;
using XTramileSolution.PracticalTest.Repository.MockRepository;
using Xunit;

namespace XTramileSolution.PracticalTest.Repository.Tests
{
    public class MockCityRepositoryTests
    {
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly MockCityRepository _mockCityRepository;
        private readonly string _testFilePath;

        public MockCityRepositoryTests()
        {
            _mockCountryRepository = new Mock<ICountryRepository>();

            // Simpan file JSON di lokasi sementara untuk unit test
            _testFilePath = Path.Combine(Path.GetTempPath(), "test_cities.json");
            File.WriteAllText(_testFilePath, "[]"); // Default kosong

            // Inject mock repository
            _mockCityRepository = new MockCityRepository(_mockCountryRepository.Object);

            // Paksa field _filePath ke file sementara untuk testing
            typeof(MockCityRepository)
                .GetField("_filePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_mockCityRepository, _testFilePath);
        }

        private void WriteTestDataToFile(IEnumerable<CityJson> cities)
        {
            var json = JsonSerializer.Serialize(cities);
            File.WriteAllText(_testFilePath, json);
        }

        [Fact]
        public async Task LoadCitiesAsync_ShouldReturnEmpty_WhenFileNotExists()
        {
            // Hapus file untuk mensimulasikan skenario file tidak ada
            File.Delete(_testFilePath);

            var cities = await _mockCityRepository.GetAllCountryCityAsync(Guid.NewGuid());

            Assert.Empty(cities);
        }

        [Fact]
        public async Task LoadCitiesAsync_ShouldReturnEmpty_WhenFileIsEmpty()
        {
            WriteTestDataToFile(new List<CityJson>());

            var cities = await _mockCityRepository.GetAllCountryCityAsync(Guid.NewGuid());

            Assert.Empty(cities);
        }

        [Fact]
        public async Task LoadCitiesAsync_ShouldReturnCities_WhenValidJson()
        {
            var countryId = Guid.NewGuid();

            // Mock country repository agar mengembalikan country yang sesuai
            _mockCountryRepository.Setup(repo => repo.GetByCodeAsync("ID")).ReturnsAsync(new CountryEntity
            {
                Id = countryId,
                Code = "ID",
                Name = "Indonesia"
            });

            WriteTestDataToFile(new List<CityJson>
            {
                new CityJson { Name = "Jakarta", CountryCode = "ID" },
                new CityJson { Name = "Bandung", CountryCode = "ID" }
            });

            var cities = await _mockCityRepository.GetAllCountryCityAsync(countryId);

            Assert.Equal(2, cities.Count());
            Assert.Contains(cities, c => c.Name == "Jakarta");
            Assert.Contains(cities, c => c.Name == "Bandung");
        }

        [Fact]
        public async Task LoadCitiesAsync_ShouldSkipCities_WhenCountryNotFound()
        {
            // Tidak ada country yang cocok dengan "US"
            _mockCountryRepository.Setup(repo => repo.GetByCodeAsync("US")).ReturnsAsync((CountryEntity)null);

            WriteTestDataToFile(new List<CityJson>
            {
                new CityJson { Name = "New York", CountryCode = "US" }
            });

            var cities = await _mockCityRepository.GetAllCountryCityAsync(Guid.NewGuid());

            Assert.Empty(cities);
        }

        [Fact]
        public async Task GetAllCountryCityAsync_ShouldReturnCitiesForSpecificCountry()
        {
            var countryId = Guid.NewGuid();

            _mockCountryRepository.Setup(repo => repo.GetByCodeAsync("JP")).ReturnsAsync(new CountryEntity
            {
                Id = countryId,
                Code = "JP",
                Name = "Japan"
            });

            WriteTestDataToFile(new List<CityJson>
            {
                new CityJson { Name = "Tokyo", CountryCode = "JP" },
                new CityJson { Name = "Osaka", CountryCode = "JP" }
            });

            var cities = await _mockCityRepository.GetAllCountryCityAsync(countryId);

            Assert.Equal(2, cities.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCity_WhenIdExists()
        {
            var countryId = Guid.NewGuid();
            var cityId = Guid.NewGuid();

            _mockCountryRepository.Setup(repo => repo.GetByCodeAsync("FR")).ReturnsAsync(new CountryEntity
            {
                Id = countryId,
                Code = "FR",
                Name = "France"
            });

            WriteTestDataToFile(new List<CityJson>
            {
                new CityJson { Name = "Paris", CountryCode = "FR" }
            });

            var cities = await _mockCityRepository.GetAllCountryCityAsync(countryId);
            var city = cities.FirstOrDefault();

            Assert.NotNull(city);
            Assert.Equal("Paris", city.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCityNotFound()
        {
            var city = await _mockCityRepository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(city);
        }
    }
}
