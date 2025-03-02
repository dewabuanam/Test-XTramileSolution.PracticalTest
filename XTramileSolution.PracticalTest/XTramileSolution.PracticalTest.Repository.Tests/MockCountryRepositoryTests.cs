using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.MockRepository;
using Xunit;

namespace XTramileSolution.PracticalTest.Repository.Tests
{
    public class MockCountryRepositoryTests
    {
        private readonly MockCountryRepository _mockCountryRepository;
        private readonly string _testFilePath;

        public MockCountryRepositoryTests()
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _testFilePath = Path.Combine(assemblyLocation ?? string.Empty, "Assets", "test_countries.json");

            // Buat file kosong sebelum testing
            Directory.CreateDirectory(Path.GetDirectoryName(_testFilePath) ?? string.Empty);
            File.WriteAllText(_testFilePath, "[]");

            // Paksa repository untuk menggunakan file testing
            _mockCountryRepository = new MockCountryRepository();
            typeof(MockCountryRepository)
                .GetField("_filePath", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(_mockCountryRepository, _testFilePath);
        }

        private void WriteTestDataToFile(Dictionary<string, string> countries)
        {
            var json = JsonSerializer.Serialize(countries);
            File.WriteAllText(_testFilePath, json);
        }

        [Fact]
        public async Task LoadCountriesAsync_ShouldReturnEmpty_WhenFileNotExists()
        {
            // Hapus file untuk mensimulasikan skenario file tidak ada
            File.Delete(_testFilePath);

            var countries = await _mockCountryRepository.GetAllAsync();

            Assert.Empty(countries);
        }

        [Fact]
        public async Task LoadCountriesAsync_ShouldReturnEmpty_WhenFileIsEmpty()
        {
            WriteTestDataToFile(new Dictionary<string, string>());

            var countries = await _mockCountryRepository.GetAllAsync();

            Assert.Empty(countries);
        }

        [Fact]
        public async Task LoadCountriesAsync_ShouldReturnEmpty_WhenInvalidJson()
        {
            File.WriteAllText(_testFilePath, "{ invalid json }");

            var countries = await _mockCountryRepository.GetAllAsync();

            Assert.Empty(countries);
        }

        [Fact]
        public async Task LoadCountriesAsync_ShouldReturnCountries_WhenValidJson()
        {
            WriteTestDataToFile(new Dictionary<string, string>
            {
                { "ID", "Indonesia" },
                { "US", "United States" }
            });

            var countries = await _mockCountryRepository.GetAllAsync();

            Assert.Equal(2, countries.Count());
            Assert.Contains(countries, c => c.Name == "Indonesia" && c.Code == "ID");
            Assert.Contains(countries, c => c.Name == "United States" && c.Code == "US");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCountries()
        {
            WriteTestDataToFile(new Dictionary<string, string>
            {
                { "JP", "Japan" },
                { "FR", "France" }
            });

            var countries = await _mockCountryRepository.GetAllAsync();

            Assert.Equal(2, countries.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCountry_WhenIdExists()
        {
            WriteTestDataToFile(new Dictionary<string, string>
            {
                { "AU", "Australia" }
            });

            var countries = await _mockCountryRepository.GetAllAsync();
            var firstCountry = countries.First();

            var result = await _mockCountryRepository.GetByIdAsync(firstCountry.Id);

            Assert.NotNull(result);
            Assert.Equal("Australia", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenIdNotExists()
        {
            var result = await _mockCountryRepository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnCorrectCountry_WhenCodeExists()
        {
            WriteTestDataToFile(new Dictionary<string, string>
            {
                { "CN", "China" }
            });

            var result = await _mockCountryRepository.GetByCodeAsync("CN");

            Assert.NotNull(result);
            Assert.Equal("China", result.Name);
        }

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnNull_WhenCodeNotExists()
        {
            var result = await _mockCountryRepository.GetByCodeAsync("XX");

            Assert.Null(result);
        }
    }
}
