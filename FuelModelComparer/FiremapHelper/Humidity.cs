using System.Text.Json.Serialization;

namespace FiremapHelper
{
    public class Humidity
    {
        [JsonPropertyName("HumidityId")]
        public int Value { get; set; }

        [JsonPropertyName("HumidityDisplay")]
        public string Text { get; set; }
    }
}
