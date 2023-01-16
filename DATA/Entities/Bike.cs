using System.Text.Json.Serialization;

namespace DATA.Entities
{
    public class Bike
    {
        [JsonPropertyName("bike_id")]
        public string? BikeId { get; set; }
        [JsonPropertyName("station_id")]
        public string? StationId { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("lon")]
        public double Longitude { get; set; }
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }
        [JsonPropertyName("is_reserved")]
        public int IsReversed { get; set; }
        [JsonPropertyName("is_disabled")]
        public int IsDisabled { get; set; }
    }

    public class BikeData
    {
        [JsonPropertyName("bikes")]
        public List<Bike> Bikes { get; set; }
    }

    public class BikeRoot
    {
        [JsonPropertyName("last_updated")]
        public int LastUpdated { get; set; }
        [JsonPropertyName("ttl")]
        public int Ttl { get; set; }
        [JsonPropertyName("data")]
        public BikeData Data { get; set; }
    }
}
