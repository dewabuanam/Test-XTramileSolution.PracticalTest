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
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<CountryResourceModel>> GetAllCountriesAsync()
        {
            var countries = await _countryRepository.GetAllAsync();
            return countries.Select(c => new CountryResourceModel
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code
            });
        }

        public async Task<CountryResourceModel> GetCountryByIdAsync(Guid id)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            return country == null ? null : new CountryResourceModel
            {
                Id = country.Id,
                Name = country.Name,
                Code = country.Code
            };
        }
    }
}
