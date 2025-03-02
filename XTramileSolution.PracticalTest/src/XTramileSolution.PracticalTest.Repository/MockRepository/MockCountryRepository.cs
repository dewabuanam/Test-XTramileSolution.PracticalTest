using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.Interface;

namespace XTramileSolution.PracticalTest.Repository.MockRepository
{
    public class MockCountryRepository : ICountryRepository
    {
        private readonly Lazy<Task<List<CountryEntity>>> _cachedCountries;
        private readonly string _filePath;

        public MockCountryRepository()
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _filePath = Path.Combine(assemblyLocation ?? string.Empty, "Assets", "countries.json");
            _cachedCountries = new Lazy<Task<List<CountryEntity>>>(LoadCountriesAsync);
        }

        private async Task<List<CountryEntity>> LoadCountriesAsync()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"File not found: {_filePath}");
                return new List<CountryEntity>();
            }

            try
            {
                var json = await File.ReadAllTextAsync(_filePath);
                var countryDict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                return countryDict.Select(c => new CountryEntity
                {
                    Id = Guid.NewGuid(),
                    Name = c.Value,
                    Code = c.Key
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading country JSON: {ex.Message}");
                return new List<CountryEntity>();
            }
        }

        public async Task<IEnumerable<CountryEntity>> GetAllAsync() => await _cachedCountries.Value;

        public async Task<CountryEntity> GetByIdAsync(Guid id)
        {
            var allCountries = await GetAllAsync();
            return allCountries.FirstOrDefault(c => c.Id == id);
        }

        public async Task<CountryEntity> GetByCodeAsync(string code)
        {
            var allCountries = await GetAllAsync();
            return allCountries.FirstOrDefault(c => c.Code == code);
        }
    }
}