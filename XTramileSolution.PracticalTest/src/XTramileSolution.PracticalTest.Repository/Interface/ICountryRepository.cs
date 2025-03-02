using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.Entity;

namespace XTramileSolution.PracticalTest.Repository.Interface
{
    public interface ICountryRepository
    {
        Task<IEnumerable<CountryEntity>> GetAllAsync();
        Task<CountryEntity> GetByIdAsync(Guid id);
        Task<CountryEntity> GetByCodeAsync(string code);
    }
}