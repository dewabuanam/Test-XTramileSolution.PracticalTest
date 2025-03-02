using System;

namespace XTramileSolution.PracticalTest.Repository.ResourceModel
{
    public class CityResourceModel
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; }
    }
}