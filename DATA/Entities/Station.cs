using System.Text.Json.Serialization;

namespace DATA.Entities
{
    public class Station
    {
        [JsonPropertyName("station_id")]
        public string StationId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("region_id")]
        public string RegionId { get; set; }
        [JsonPropertyName("lon")]
        public double Longitude { get; set; }
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("rental_methods")]
        public List<string> RentalMethods { get; set; }
    }

    public class StationData
    {
        [JsonPropertyName("stations")]
        public List<Station> Stations { get; set; }
    }

    public class StationRoot
    {
        [JsonPropertyName("last_updated")]
        public int LastUpdated { get; set; }
        [JsonPropertyName("ttl")]
        public int Ttl { get; set; }
        [JsonPropertyName("data")]
        public StationData Data { get; set; }
    }
}
