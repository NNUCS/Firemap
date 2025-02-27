using System;
using System.Text.Json.Serialization;


namespace FiremapHelper
{
    public class TreeCover
    {
        [JsonPropertyName("TreeCoverID")]
        public int Value { get; set; }

        [JsonPropertyName("TreeCoverDisplay")]
        public string Text { get; set; }
    }
}
