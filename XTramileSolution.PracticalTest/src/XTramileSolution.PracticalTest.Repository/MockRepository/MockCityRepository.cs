using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Repository.JsonResponse;

namespace XTramileSolution.PracticalTest.Repository.MockRepository
{
    public class MockCityRepository : ICityRepository
    {
        private readonly Lazy<Task<List<CityEntity>>> _cachedCities;
        private readonly string _filePath;
        private readonly ICountryRepository _countryRepository;

        public MockCityRepository(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _filePath = Path.Combine(assemblyLocation ?? string.Empty, "Assets", "cities.json");
            _cachedCities = new Lazy<Task<List<CityEntity>>>(LoadCitiesAsync);
        }

        private async Task<List<CityEntity>> LoadCitiesAsync()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"File not found: {_filePath}");
                return new List<CityEntity>();
            }

            try
            {
                var json = await File.ReadAllTextAsync(_filePath);
                var citiesJson = JsonSerializer.Deserialize<IEnumerable<CityJson>>(json);

                var listCity = new List<CityEntity>();
                foreach (var city in citiesJson)
                {
                    var country = await _countryRepository.GetByCodeAsync(city.CountryCode);
                    if (country==null)
                    {
                        Console.WriteLine($"Country not found: {city.CountryCode}");
                        continue;
                    }
                    listCity.Add(new CityEntity
                    {
                        Id = Guid.NewGuid(),
                        CountryId = country.Id,
                        Name = city.Name
                    });
                }
                return listCity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading country JSON: {ex.Message}");
                return new List<CityEntity>();
            }
        }

        public async Task<IEnumerable<CityEntity>> GetAllCountryCityAsync(Guid countryId)
        {
            var allCities = await _cachedCities.Value;
            return allCities.Where(c => c.CountryId == countryId);
        }

        public async Task<CityEntity> GetByIdAsync(Guid id)
        {
            var allCities = await _cachedCities.Value;
            return allCities.FirstOrDefault(c => c.Id == id);
        }
    }
}