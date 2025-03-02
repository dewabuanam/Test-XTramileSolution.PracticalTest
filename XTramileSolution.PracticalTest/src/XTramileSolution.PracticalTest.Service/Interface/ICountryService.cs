using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.ServiceModel;

namespace XTramileSolution.PracticalTest.Service.Interface
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryResourceModel>> GetAllCountriesAsync();
        Task<CountryResourceModel> GetCountryByIdAsync(Guid id);
    }
}