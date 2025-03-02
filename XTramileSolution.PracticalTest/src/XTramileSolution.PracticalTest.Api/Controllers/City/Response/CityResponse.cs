using System;

namespace XTramileSolution.PracticalTest.Api.Controllers.City.Response
{
    public class CityResponse
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}