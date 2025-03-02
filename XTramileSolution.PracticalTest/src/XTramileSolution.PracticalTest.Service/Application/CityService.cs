using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.Entity;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.Interface;
using XTramileSolution.PracticalTest.Service.ServiceModel;

namespace XTramileSolution.PracticalTest.Service.Application
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<CityResourceModel>> GetAllCountryCityAsync(Guid countryId)
        {
            var cities = await _cityRepository.GetAllCountryCityAsync(countryId);
            return cities.Select(c => new CityResourceModel
            {
                Id = c.Id,
                CountryId = c.CountryId,
                Name = c.Name
            });
        }

        public async Task<CityResourceModel> GetCityByIdAsync(Guid id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            return city == null ? null : new CityResourceModel
            {
                Id = city.Id,
                CountryId = city.CountryId,
                Name = city.Name,
            };
        }
    }
}
