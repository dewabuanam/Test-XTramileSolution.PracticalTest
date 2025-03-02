using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.Entity;

namespace XTramileSolution.PracticalTest.Repository.Interface
{
    public interface ICityRepository
    {
        Task<IEnumerable<CityEntity>> GetAllCountryCityAsync(Guid countryId);
        Task<CityEntity> GetByIdAsync(Guid id);
    }
}