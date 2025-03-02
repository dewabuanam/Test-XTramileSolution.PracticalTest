using System;

namespace XTramileSolution.PracticalTest.Repository.Entity
{
    public class CountryEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Code { get; set; }
    }
}