using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XTramileSolution.PracticalTest.Api.Controllers.City.Response;
using XTramileSolution.PracticalTest.Service.Interface;

namespace XTramileSolution.PracticalTest.Api.Controllers.City
{
    [ApiController]
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("{countryId}")]
        public async Task<IEnumerable<CityResponse>> GetCountryCity(Guid countryId)
        {
            var countries = await _cityService.GetAllCountryCityAsync(countryId);
            return countries.Select(c=> new CityResponse()
            {
                Id = c.Id,
                CountryId = c.CountryId,
                Name = c.Name
            });
        }
    }
}
