using System.Text.Json.Serialization;

namespace FiremapHelper
{
    public class FuelModel
    {
        [JsonPropertyName("FiremapInputCode")]
        public string Text { get; set; }

        [JsonPropertyName("FiremapInputId")]
        public int Value { get; set; }

    }
}
