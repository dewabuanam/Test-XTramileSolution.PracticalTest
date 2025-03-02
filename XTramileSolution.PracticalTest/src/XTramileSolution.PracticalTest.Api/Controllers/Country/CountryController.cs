using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XTramileSolution.PracticalTest.Api.Controllers.Country.Response;
using XTramileSolution.PracticalTest.Service.Interface;

namespace XTramileSolution.PracticalTest.Api.Controllers.Country
{
    [ApiController]
    [Route("api/countries")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IEnumerable<CountryResponse>> Get()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return countries.Select(c=> new CountryResponse()
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryResponse>> Get(Guid id)
        {
            var country = await _countryService.GetCountryByIdAsync(id);
            if (country == null) return NotFound();
            return new CountryResponse()
            {
                Id = country.Id,
                Name = country.Name,
                Code = country.Code
            };
        }
    }
}
