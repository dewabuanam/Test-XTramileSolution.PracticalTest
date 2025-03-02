using System;
using System.Threading.Tasks;
using XTramileSolution.PracticalTest.Repository.ResourceModel;
using XTramileSolution.PracticalTest.Service.ResourceModel;

namespace XTramileSolution.PracticalTest.Service.Interface
{
    public interface IWeatherService
    {
        Task<WeatherResourceModel> GetWeatherByCityAsync(Guid cityId);
    }
}