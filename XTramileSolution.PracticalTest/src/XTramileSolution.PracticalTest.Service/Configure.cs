using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XTramileSolution.PracticalTest.Repository;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Repository.MockRepository;
using XTramileSolution.PracticalTest.Service.Application;
using XTramileSolution.PracticalTest.Service.Interface;

namespace XTramileSolution.PracticalTest.Service
{
    public static class Configure
    {
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IWeatherService, WeatherService>();
            
            services.AddRepositories(configuration);
            
            return services;
        }
    }
}