using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiremapHelper
{
    public class FiremapOption
    {
        [JsonPropertyName("OutputRefId")]
        public int Value { get; set; }

        [JsonPropertyName("OutputDisplay")]
        public string Text { get; set; }
    }
}
