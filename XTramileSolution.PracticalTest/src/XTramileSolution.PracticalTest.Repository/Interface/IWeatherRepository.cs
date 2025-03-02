using System;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.Entity;

namespace XTramileSolution.PracticalTest.Repository.Interface
{
    public interface IWeatherRepository
    {
        Task<WeatherEntity> GetWeatherByCityAsync(Guid cityId);
    }
}