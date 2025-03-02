using System.Text.Json.Serialization;

namespace XTramileSolution.PracticalTest.Repository.JsonResponse
{
    public class CityJson
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("country")]
        public string CountryCode { get; set; }
    }
}