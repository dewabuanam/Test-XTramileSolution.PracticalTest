using System;

namespace XTramileSolution.PracticalTest.Repository.Entity
{
    public class CityEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CountryId { get; set; }
        public string Name { get; set; }
    }
}