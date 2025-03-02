using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XTramileSolution.PracticalTest.Repository.Domain;
using XTramileSolution.PracticalTest.Repository.Interface;
using XTramileSolution.PracticalTest.Repository.MockRepository;
using XTramileSolution.PracticalTest.Repository.ThirdPartyApi;

namespace XTramileSolution.PracticalTest.Repository
{
    public static class Configure
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WeatherApiOptions>(configuration.GetSection("WeatherApi"));
            services.AddSingleton<ICountryRepository, MockCountryRepository>();
            services.AddSingleton<ICityRepository, MockCityRepository>();
            services.AddScoped<IWeatherRepository, OpenWeatherRepository>();

            return services;
        }
    }
}