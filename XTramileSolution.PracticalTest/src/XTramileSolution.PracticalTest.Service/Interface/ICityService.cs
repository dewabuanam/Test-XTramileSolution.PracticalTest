using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.ResourceModel;

namespace XTramileSolution.PracticalTest.Service.Interface
{
    public interface ICityService
    {
        Task<IEnumerable<CityResourceModel>> GetAllCountryCityAsync(Guid countryId);
        Task<CityResourceModel> GetCityByIdAsync(Guid id);
    }
}